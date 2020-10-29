using FrostAura.Services.Devices.Data;
using FrostAura.Services.Devices.Data.GraphQl.Types;
using FrostAura.Services.Devices.Data.Models.Entities;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using System.Linq;

namespace FrostAura.Services.Devices.Shared.Models.GraphQl
{
    /// <summary>
    /// GraphQL shema queries.
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
        /// Attributes query for GraphQL.
        /// </summary>
        /// <param name="db">Database context.</param>
        /// <returns>Collection of queryable attributes.</returns>
        [UsePaging(SchemaType = typeof(AttributeType))]
        [UseSelection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Attribute> Attributes([Service] DevicesDbContext db) => db.Attributes;
    }
}
