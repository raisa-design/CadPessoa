﻿using CadPessoa.Api.Configuration;
using CadPessoa.Api.Data;
using Microsoft.Extensions.Configuration;

namespace CadPessoa.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostEnvironment hostEnvironment)
        {


            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if (hostEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddIdentityConfiguration(Configuration);

            services.AddApiConfiguration();

            services.AddSwaggerConfiguration();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerConfiguration();

            app.UseApiConfiguration(env);

            app.UseCors(option => option.AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowAnyOrigin());
        }
    }
}
