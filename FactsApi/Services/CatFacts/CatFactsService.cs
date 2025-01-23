using FactsApi.Services.CatFacts.DTO;
using FactsApi.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;

namespace FactsApi.Services.CatFacts
{
    public class CatFactsService : ICatFactsService
    {
        /// <summary>
        /// Service for retrieving facts about cats from an external API.
        /// </summary>
        private readonly ServiceSettings serviceSettings;
        private readonly ILogger logger;
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatFactsService"/> class.
        /// </summary>
        /// <param name="serviceSettings">The settings for external services, including the Cat Facts API URL.</param>
        /// <param name="logger">The logger for capturing application logs.</param>
        /// <param name="httpClientFactory"></param>
        public CatFactsService(IOptions<ServiceSettings> serviceSettings, ILogger<CatFactsService> logger, IHttpClientFactory httpClientFactory)
        {
            this.serviceSettings = serviceSettings.Value;
            this.logger = logger;
            this.httpClient = httpClientFactory.CreateClient();
        }

        /// <summary>
        /// Retrieves a list of cat facts from the external Cat Facts API.
        /// </summary>
        /// <param name="limit">The maximum number of cat facts to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation, containing a <see cref="FactsContainer"/> with the retrieved cat facts.</returns>
        /// <exception cref="HttpRequestException">Thrown if the HTTP request fails or the response status is not successful.</exception>
        /// <exception cref="JsonException">Thrown if the response content cannot be deserialized into the expected format.</exception>
        /// <exception cref="Exception">Thrown for any other errors during the process.</exception>
        public async Task<FactsContainer> GetFactsAsync(int limit)
        {
            var url = $"{serviceSettings.CatFacts}/facts?limit={limit}";
            try
            {
                logger.LogDebug($"Call to :{url}");
                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    logger.LogError($"Failed to get cat facts. Status code: {response.StatusCode}");

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();

                var catFactsResponse = JsonSerializer.Deserialize<CatFactsResponse>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
                
                return new FactsContainer
                {
                    Facts = catFactsResponse?.Data?.Select(s => new Fact
                    {
                        Text = s.Fact,
                        Category = "Cats"
                    })
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while fetching cat facts: {ex.Message}");

                throw;
            }
        }

    }
}
