﻿using FactsApi.Services.NinjaFacts.DTO;
using FactsApi.Services.NinjaFacts;
using Microsoft.Extensions.Options;
using System.Text.Json;
using FactsApi.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FactsApi.Services.NinjaFacts
{
    public class NinjaFactsService : INinjaFactsService
    {
        /// <summary>
        /// Service for retrieving ninja facts from an external API.
        /// </summary>
        private readonly ServiceSettings serviceSettings;
        private readonly ILogger logger;
        private readonly HttpClient httpClient;
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjaFactsService"/> class.
        /// </summary>
        /// <param name="serviceSettings">The settings for external services, including the Ninja Facts API URL and API key.</param>
        /// <param name="logger">The logger for capturing application logs.</param>
        /// <param name="httpClientFactory">Factory for creating HttpClient instances.</param>
        /// <param name="memoryCache"></param>
        public NinjaFactsService(
            IOptions<ServiceSettings> serviceSettings,
            ILogger<NinjaFactsService> logger,
            IHttpClientFactory httpClientFactory,
            IMemoryCache memoryCache)
        {
            this.serviceSettings = serviceSettings.Value;
            this.logger = logger;
            this.httpClient = httpClientFactory.CreateClient();
            this.httpClient.DefaultRequestHeaders.Add("x-api-key", this.serviceSettings.NinjaFactsApiKey);
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// Retrieves a list of ninja facts from the external Ninja Facts API.
        /// </summary>
        /// <param name="limit">The maximum number of ninja facts to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a <see cref="FactsContainer"/>
        /// with the retrieved ninja facts.
        /// </returns>
        /// <exception cref="HttpRequestException">Thrown if the HTTP request fails or the response status is not successful.</exception>
        /// <exception cref="JsonException">Thrown if the response content cannot be deserialized into the expected format.</exception>
        /// <exception cref="Exception">Thrown for any other errors during the process.</exception>
        public async Task<FactsContainer> GetFactsAsync(int limit)
        {
            var cacheKey = $"NinjaFacts_{limit}";

            // Check if the data is already cached
            if (memoryCache.TryGetValue(cacheKey, out FactsContainer cachedFacts))
            {
                logger.LogDebug("Returning ninja facts from cache.");
                return cachedFacts;
            }

            var url = $"{serviceSettings.NinjaFacts}/facts";
            try
            {
                logger.LogDebug($"Call to :{url}");

                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogError($"Failed to get ninja facts. Status code: {response.StatusCode}");
                    response.EnsureSuccessStatusCode();
                }

                var jsonString = await response.Content.ReadAsStringAsync();

                var ninjaFactsResponse = JsonSerializer.Deserialize<IEnumerable<NinjaFact>>(
                    jsonString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var ninjaFactsResponseWithLimit = ninjaFactsResponse.Take(limit);

                var facts = new FactsContainer
                {
                    Facts = ninjaFactsResponseWithLimit.Select(s => new Fact
                    {
                        Text = s.Fact,
                        Category = "Ninjas"
                    })
                };

                // Cache the result
                memoryCache.Set(cacheKey, facts, TimeSpan.FromMinutes(10));
                logger.LogDebug("Ninja facts added to cache.");

                return facts;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while fetching ninja facts: {ex.Message}");
                throw;
            }
        }
    }
}
