using Contracts;
using Tools.Extension.String;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers.Abstract
{
    public class ControllerAbstract : Controller
    {
        private readonly ILoggerManager _Logger;
        public ControllerAbstract(ILoggerManager Logger)
        {
            _Logger = Logger;
        }

        [NonAction]
        protected string? GetRequestIP(bool tryUseXForwardHeader = true)
        {
            string? ip = null;
            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For
            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
                ip = GetHeaderValueAs<string>("X-Forwarded-For").SplitCsv()!.FirstOrDefault();
            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (ip.IsNullOrWhitespace() && HttpContext?.Connection?.RemoteIpAddress != null)
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
            if (ip.IsNullOrWhitespace())
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");
            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.
            if (ip.IsNullOrWhitespace())
                throw new Exception("Unable to determine caller's IP.");
            return ip;
        }

        private T GetHeaderValueAs<T>(string headerName)
        {
            StringValues values = string.Empty;
            if (HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString(); // writes out as Csv when there are multiple.
                if (!rawValues.IsNullOrWhitespace())
                {
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
                }
            }

            return default(T)!;
        }

        protected IActionResult ResponseAction(object response, bool erro, List<string>? messages = null)
        {
            return Ok(erro ? new ResponseModel()
            {Operation = false, Messages = messages} : new ResponseModel()
            {Operation = true, Messages = new List<string>()
            {"Success"}, Data = response});
        }
    }
}