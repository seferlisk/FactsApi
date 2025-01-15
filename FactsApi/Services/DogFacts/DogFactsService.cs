using FactsApi.Services.DogFacts.DTO;
using FactsApi.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FactsApi.Services.DogFacts
{
    public class DogFactsService : IDogFactsService
    {
        private readonly ServiceSettings serviceSettings;
        private readonly ILogger logger;

        public DogFactsService(IOptions<ServiceSettings> serviceSettings, ILogger<DogFactsService> logger)
        {
            this.serviceSettings = serviceSettings.Value;
            this.logger = logger;
        }

        public async Task<FactsContainer> GetFactsAsync(int limit)
        {
            var url = $"{serviceSettings.DogFacts}/facts?limit={limit}";
            try
            {
                logger.LogDebug($"Call to :{url}");
                var response = await new HttpClient().GetAsync(url);

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
