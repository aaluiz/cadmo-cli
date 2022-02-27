using AutoMapper;
using Contracts;
using Entities.AutoMapper;
using Entities.Data;
using Api.Models;
using Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"Something going wrong: {contextFeature.Error}");
                        await context.Response.WriteAsync(new DetailErro()
                        {Code = context.Response.StatusCode, Message = new List<string>()
                        {contextFeature.Error.Message}}.ToString());
                    }
                });
            });
        }

        public static void ConfigureSqlserverConnection(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<ApiDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Api")));
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}