using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using FrostAura.Services.Devices.Core.Extensions;
using FrostAura.Services.Devices.Data.Extensions;
using FrostAura.Services.Devices.Core.Interfaces;
using FrostAura.Services.Devices.Shared.Models;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Collections.Generic;

namespace FrostAura.Services.Devices.Api
{
    /// <summary>
    /// Entry class to the application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Application configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Overloaded constructor to allow for injecting depencies.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Added services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddFrostAuraCore(Configuration)
                .AddFrostAuraResources(Configuration)
                .AddSwaggerGen()
                .AddControllers();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Hosting environment context.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(Configuration.GetValue<string>("Documentation:Url"), Configuration.GetValue<string>("Documentation:Name"));
                });
            serviceProvider
                .GetService<IMqttManager>()?
                .InitializeAsync(CancellationToken.None)
                .GetAwaiter()
                .GetResult();
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
