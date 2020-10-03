using FrostAura.Libraries.Core.Extensions.Validation;
using FrostAura.Services.Devices.Data.Models.Entities;
using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        /// <param name="token">Cancellation token.</param>
        /// <param name="options">Jwt options.</param>
        /// <returns>Newly upserted device.</returns>
        [SubscribeAndResolve]
        public async ValueTask<IAsyncEnumerable<Device>> OnUpsertDevice(string accessToken, [Service]ITopicEventReceiver receiver, CancellationToken token, [Service]IOptionsMonitor<JwtBearerOptions> options)
        {
            accessToken.ThrowIfNullOrWhitespace(nameof(accessToken));

            if (!await IsUserAuthenticatedAsync(options.CurrentValue, accessToken, token)) throw new ArgumentException("Unauthorized");

            return await receiver.SubscribeAsync<string, Device>("devices", token);
        }

        /// <summary>
        /// Subscription for when fresh attribute value(s) are upserted.
        /// </summary>
        /// <param name="deviceId">Device id to subscribe to.</param>
        /// <param name="accessToken">JWT access token.</param>
        /// <param name="receiver">Event receiver.</param>
        /// <param name="token">Cancellation token.</param>
        /// <param name="options">Jwt options.</param>
        /// <returns>Newly upserted attribute value.</returns>
        [SubscribeAndResolve]
        public async ValueTask<IAsyncEnumerable<DeviceAttribute>> OnUpsertAttributeValueForDevice(int deviceId, string accessToken, [Service]ITopicEventReceiver receiver, CancellationToken token, [Service] IOptionsMonitor<JwtBearerOptions> options)
        {
            accessToken.ThrowIfNullOrWhitespace(nameof(accessToken));

            if (!await IsUserAuthenticatedAsync(options.CurrentValue, accessToken, token)) throw new ArgumentException("Unauthorized");

            return await receiver.SubscribeAsync<string, DeviceAttribute>($"device.{deviceId}.attributes", token);
        }

        /// <summary>
        /// Determine whether a user is authenticated.
        /// </summary>
        /// <param name="options">Jwt options.</param>
        /// <param name="accessToken">Access Access</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Whether the user is authenticated.</returns>
        private async Task<bool> IsUserAuthenticatedAsync(JwtBearerOptions options, string accessToken, CancellationToken token)
        {
            OpenIdConnectConfiguration configuration = null;

            if (options.ConfigurationManager != null)
            {
                configuration = await options.ConfigurationManager.GetConfigurationAsync(token);
            }

            var validationParameters = options.TokenValidationParameters.Clone();
            
            if (configuration != null)
            {
                var issuers = new[] { configuration.Issuer };

                validationParameters.ValidIssuers = validationParameters.ValidIssuers?.Concat(issuers) ?? issuers;
                validationParameters.IssuerSigningKeys = validationParameters.IssuerSigningKeys?.Concat(configuration.SigningKeys)
                    ?? configuration.SigningKeys;
            }

            foreach (var validator in options.SecurityTokenValidators)
            {
                if (validator.CanReadToken(accessToken))
                {
                    try
                    {
                        var principal = validator.ValidateToken(accessToken, validationParameters, out SecurityToken validatedToken);

                        return true;
                    }
                    catch (Exception ex)
                    { }
                }

                return false;
            }

            return true;
        }
    }
}
