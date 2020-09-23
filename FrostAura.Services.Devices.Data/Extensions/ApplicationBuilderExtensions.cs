using FrostAura.Libraries.Core.Extensions.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FrostAura.Services.Devices.Data.Extensions
{
    /// <summary>
    /// Application builder extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Initialize database context sync.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <returns>Application builder.</returns>
        public static IApplicationBuilder UseFrostAuraResources<TCaller>(this IApplicationBuilder app)
        {
            var RESILIENT_ALLOWED_ATTEMPTS = 3;
            var RESILIENT_BACKOFF = TimeSpan.FromSeconds(5);

            for (int i = 1; i <= RESILIENT_ALLOWED_ATTEMPTS; i++)
            {
                try
                {
                    var response = InitializeDatabasesAsync<TCaller>(app).GetAwaiter().GetResult();

                    return response;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Database migration failed on try {i}.");
                    Thread.Sleep(RESILIENT_BACKOFF);
                }
            }

            return null;
        }

        /// <summary>
        /// Initialize database context async.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <returns>Application builder.</returns>
        private static async Task<IApplicationBuilder> InitializeDatabasesAsync<TCaller>(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var logger = serviceScope
                    .ServiceProvider
                    .GetRequiredService<ILogger<TCaller>>()
                    .ThrowIfNull("Logger");
                var devicesDbContext = serviceScope
                    .ServiceProvider
                    .GetRequiredService<DevicesDbContext>();

                logger.LogInformation($"Migrating database '{nameof(devicesDbContext)}' => '{devicesDbContext.Database.GetDbConnection().ConnectionString}'.");

                devicesDbContext
                    .Database
                    .Migrate();


                // Seed data goes here.

                await devicesDbContext.SaveChangesAsync();
            }

            return app;
        }
    }
}
