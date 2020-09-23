using FrostAura.Services.Devices.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FrostAura.Services.Devices.Data
{
    /// <summary>
    /// Devices database context.
    /// </summary>
    public class DevicesDbContext : DbContext
    {
        /// <summary>
        /// Construct and allow for passing options.
        /// </summary>
        /// <param name="options">Db creation options.</param>
        public DevicesDbContext(DbContextOptions<DevicesDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Device>()
                .HasIndex(d => d.Name)
                .IsUnique(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}