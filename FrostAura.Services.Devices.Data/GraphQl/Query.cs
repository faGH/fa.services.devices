using FrostAura.Libraries.Core.Extensions.Validation;
using FrostAura.Services.Devices.Data;
using FrostAura.Services.Devices.Data.GraphQl.Types;
using FrostAura.Services.Devices.Data.Models.Entities;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FrostAura.Services.Devices.Shared.Models.GraphQl
{
    /// <summary>
    /// GraphQL shema query.
    /// </summary>
    public class Query
    {
        /// <summary>
        /// Devices query for GraphQL.
        /// </summary>
        /// <param name="db">Database context.</param>
        /// <returns>Collection of queryable devices.</returns>
        [UsePaging(SchemaType = typeof(DeviceType))]
        [UseSelection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Device> Devices([Service] DevicesDbContext db) => db.Devices;

        /// <summary>
        /// Device query for GraphQL.
        /// </summary>
        /// <param name="db">Database context.</param>
        /// <param name="id">Unique auto-genenrated id for the device.</param>
        /// <returns>Collection of queryable devices.</returns>
        [UseFiltering]
        [UseSorting]
        public async Task<Device> Device([Service] DevicesDbContext db, int id)
        {
            var device = await db
                .Devices
                .Include(d => d.Attributes)
                .ThenInclude(a => a.Attribute)
                .FirstOrDefaultAsync(d => d.Id == id);

            return device
                .ThrowIfNull(nameof(device));
        }

        /// <summary>
        /// Attributes query for GraphQL.
        /// </summary>
        /// <param name="db">Database context.</param>
        /// <returns>Collection of queryable attributes.</returns>
        [UsePaging(SchemaType = typeof(AttributeType))]
        [UseSelection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Attribute> Attributes([Service] DevicesDbContext db) => db.Attributes;

        /// <summary>
        /// Attribute query for GraphQL.
        /// </summary>
        /// <param name="db">Database context.</param>
        /// <param name="id">Unique auto-genenrated id for the attribute.</param>
        /// <returns>Attribute or null.</returns>
        [UseFiltering]
        [UseSorting]
        public async Task<Attribute> Attribute([Service] DevicesDbContext db, int id)
        {
            var attribute = await db
                .Attributes
                .Include(a => a.DeviceAttributes)
                .ThenInclude(da => da.Device)
                .FirstAsync(d => d.Id == id);

            return attribute;
        }
    }
}
