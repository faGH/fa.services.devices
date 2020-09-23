using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace FrostAura.Services.Devices.Data.Factories.DesignTime
{
    /// <summary>
    /// DB context factory for running migrations in design time.
    /// This allows for running migrations in the .Data project independently.
    /// </summary>
    public class DevicesDbContextDesignTimeFactory : IDesignTimeDbContextFactory<DevicesDbContext>
    {
        /// <summary>
        /// Factory method for producing the design time db context
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Database context.</returns>
        public DevicesDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Migrations.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DevicesDbContext>();
            var connectionString = configuration
                .GetConnectionString("DevicesDbConnection");

            builder.UseSqlServer(connectionString);

            Console.WriteLine($"Used connection string for configuration db: {connectionString}");

            return new DevicesDbContext(builder.Options);
        }
    }
}
