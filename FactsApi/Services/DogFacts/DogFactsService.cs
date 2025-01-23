using FactsApi.Services.DogFacts.DTO;
using FactsApi.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FactsApi.Services.DogFacts
{
    /// <summary>
    /// Service for retrieving facts about dogs from an external API.
    /// </summary>
    public class DogFactsService : IDogFactsService
    {
        private readonly ServiceSettings serviceSettings;
        private readonly ILogger logger;
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="DogFactsService"/> class.
        /// </summary>
        /// <param name="serviceSettings">The settings for external services, including the Dog Facts API URL.</param>
        /// <param name="logger">The logger for capturing application logs.</param>
        /// <param name="httpClientFactory">Factory for creating HttpClient instances.</param>
        public DogFactsService(IOptions<ServiceSettings> serviceSettings, ILogger<DogFactsService> logger, IHttpClientFactory httpClientFactory)
        {
            this.serviceSettings = serviceSettings.Value;
            this.logger = logger;
            this.httpClient = httpClientFactory.CreateClient();
        }

        /// <summary>
        /// Retrieves a list of dog facts from the external Dog Facts API.
        /// </summary>
        /// <param name="limit">The maximum number of dog facts to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a <see cref="FactsContainer"/> 
        /// with the retrieved dog facts.
        /// </returns>
        /// <exception cref="HttpRequestException">Thrown if the HTTP request fails or the response status is not successful.</exception>
        /// <exception cref="JsonException">Thrown if the response content cannot be deserialized into the expected format.</exception>
        /// <exception cref="Exception">Thrown for any other errors during the process.</exception>
        public async Task<FactsContainer> GetFactsAsync(int limit)
        {
            var url = $"{serviceSettings.DogFacts}/facts?limit={limit}";
            try
            {
                logger.LogDebug($"Call to :{url}");
                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    logger.LogError($"Failed to get Dog facts. Status code: {response.StatusCode}");

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();

                var dogFactsResponse = JsonSerializer.Deserialize<DogsFactsResponse>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                


                return new FactsContainer
                {
                    Facts = dogFactsResponse?.Data?.Select(s => new Fact
                    {
                        Text = s?.Attributes?.Body,
                        Category = "Dogs"
                    })
                };

            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while fetching dog facts: {ex.Message}");

                throw;
            }
        }
    }          
}
