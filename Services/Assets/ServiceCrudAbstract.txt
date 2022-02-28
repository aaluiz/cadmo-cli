using AutoMapper;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Services.Abstract
{
    public partial class ServiceCrudAbstract
    {
        protected readonly IMapper _Mapper;
        protected IConfiguration _Configuration;
        public bool HasErro => (_ErrorMessages != null && _ErrorMessages.Count > 0);
        protected List<string>? _ErrorMessages { get; set; }

        public List<string>? ErrorMessages => _ErrorMessages;
        public ServiceCrudAbstract(IMapper Mapper, IConfiguration Configuration)
        {
            _Mapper = Mapper;
            _Configuration = Configuration;
        }

        protected void CleanErrors()
        {
            _ErrorMessages = new List<string>();
        }

        protected void SetErro(string Mensagem)
        {
            _ErrorMessages!.Add(Mensagem);
        }

        protected static string ConvertToJson(object Model)
        {
            return JsonSerializer.Serialize(Model);
        }

        public dest MapTo<src, dest>(src registro)
        {
            return _Mapper.Map<src, dest>(registro);
        }

        public List<dest> MapToAll<src, dest>(List<src> registros)
        {
            var listView = new List<dest>();
            if (registros?.Count > 0)
            {
                foreach (var registro in registros)
                {
                    listView.Add(MapTo<src, dest>(registro));
                }
            }

            return listView;
        }

        protected bool IsValidEmail(string Email)
        {
            string emailRegex = string.Format("{0}{1}", @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))", @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
            bool emailValido;
            try
            {
                emailValido = Regex.IsMatch(Email, emailRegex);
            }
            catch (RegexMatchTimeoutException)
            {
                emailValido = false;
            }

            return emailValido;
        }
    }
}