using FrostAura.Libraries.Core.Extensions.Validation;
using FrostAura.Services.Devices.Data.Models.Entities;
using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrostAura.Services.Devices.Shared.Models.GraphQl
{
    /// <summary>
    /// GraphQL shema subscriptions.
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// Subscription for when a device is upserted.
        /// </summary>
        /// <param name="accessToken">JWT access token.</param>
        /// <param name="receiver">Event receiver.</param>
        /// <returns>Newly upserted device.</returns>
        [SubscribeAndResolve]
        public async ValueTask<IAsyncEnumerable<Device>> OnUpsertDevice(string accessToken, [Service]ITopicEventReceiver receiver)
        {
            accessToken
                .ThrowIfNullOrWhitespace(nameof(accessToken));

            return await receiver.SubscribeAsync<string, Device>("devices");
        }

        /// <summary>
        /// Subscription for when fresh attribute value(s) are upserted.
        /// </summary>
        /// <param name="accessToken">JWT access token.</param>
        /// <param name="deviceId">Device id to subscribe to.</param>
        /// <param name="receiver">Event receiver.</param>
        /// <returns>Newly upserted attribute value.</returns>
        [SubscribeAndResolve]
        public async ValueTask<IAsyncEnumerable<DeviceAttribute>> OnUpsertAttributeValueForDevice(string accessToken, int deviceId, [Service]ITopicEventReceiver receiver)
        {
            accessToken
                .ThrowIfNullOrWhitespace(nameof(accessToken));

            return await receiver.SubscribeAsync<string, DeviceAttribute>($"device.{deviceId}.attributes");
        }
    }
}
