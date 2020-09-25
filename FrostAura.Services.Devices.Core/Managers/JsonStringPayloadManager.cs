using FrostAura.Libraries.Core.Extensions.Validation;
using FrostAura.Services.Devices.Core.Interfaces;
using FrostAura.Services.Devices.Shared.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FrostAura.Services.Devices.Core.Managers
{
    /// <summary>
    /// Payload manager for stringified objects (JSON objects) payloads.
    /// </summary>
    public class JsonStringPayloadManager : IPayloadManager
    {
        /// <summary>
        /// Instance logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor to provide dependencies.
        /// </summary>
        /// <param name="logger">Instance logger.</param>
        public JsonStringPayloadManager(ILogger<JsonStringPayloadManager> logger)
        {
            _logger = logger
                .ThrowIfNull(nameof(logger));
        }

        /// <summary>
        /// Convert a given payload to a dictionary of mapped attributesbased on config.
        /// </summary>
        /// <param name="payload">Original payload.</param>
        /// <param name="mappings">Mappings configuration.</param>
        /// <returns>Identifier value from mappings, Mapped dictionary payload.</returns>
        public (string identifier, IDictionary<string, string> attributes) ToMappedDictionary(object payload, IList<AttributeMappingConfig> mappings)
        {
            var stringPayload = (payload as string)
                .ThrowIfNullOrWhitespace(nameof(payload));
            var normalizedStringPayload = NormalizeStringPayload(stringPayload);
            var result = new Dictionary<string, string>();

            if (!mappings.Any())
            {
                _logger.LogDebug($"No attribute mappings found.");

                return (string.Empty, result);
            }

            // Parse the string payload as JObject.
            var parsedPayload = JObject.Parse(normalizedStringPayload);

            // For each mapping, try and access the value on the JObject. If exists, find the desitnation name from the mappings
            foreach (var mapping in mappings)
            {
                var value = parsedPayload
                    .SelectToken(mapping.Source)?
                    .ToString();

                if (string.IsNullOrWhiteSpace(mapping.Destination)) continue;

                result[mapping.Destination] = value;
            }

            _logger.LogDebug($"Mapped payload '{normalizedStringPayload}' to dictionary '{JsonConvert.SerializeObject(result)}'.");

            var identifierSource = mappings
                .First(m => m.IsDeviceIdentifier)
                .Source;
            var identifierValue = parsedPayload
                .SelectToken(identifierSource)?
                .ToString();

            return (identifierValue, result);
        }

        /// <summary>
        /// Normalize / clean up a given payload.
        /// </summary>
        /// <param name="payload">Original payload.</param>
        /// <returns>Normalized payload.</returns>
        private string NormalizeStringPayload(string payload)
        {
            // Remove date time fields that are not in a stringified format as they break JSON parsing.
            var regexString = "(?<=:)(?!\")(?!\\[).+? .+?(?=,)";
            var invalidFieldsRegex = new Regex(regexString);
            var result = payload;

            foreach (Match match in invalidFieldsRegex.Matches(payload))
            {
                var invalidItem = match.Value;

                result = result.Replace(invalidItem, $"\"{invalidItem}\"");
            }

            _logger.LogDebug($"Normalized payload from '{payload}' to '{result}'.");

            return result;
        }
    }
}
