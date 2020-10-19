using JuntosCodeChallenge.API.Services;
using JuntosCodeChallenge.Domain.Customer.Interfaces;
using JuntosCodeChallenge.Infrastructure.CrossCutting;
using JuntosCodeChallenge.Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace JuntosCodeChallenge.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddControllers();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMemoryCache memoryCache, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            LogHelper log = new LogHelper();

            if (Convert.ToBoolean(Configuration[$"AppSettings:LoggingInFile"]))
                log.InitializeFile(loggerFactory, Configuration[$"AppSettings:LogginFilePath"], Configuration[$"AppSettings:LogginFileExtension"]);

            try
            {
                //Iniciliza o cache com os clientes
                new CustomerService(memoryCache, Configuration).Initialize();
            }
            catch (System.Exception e)
            {
                log.Error(e, "Erro ao carregar os clientes");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
