using FactsApi.Services.CatFacts.DTO;
using FactsApi.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FactsApi.Services.CatFacts
{
    public class CatFactsService : ICatFactsService
    {
        private readonly ServiceSettings serviceSettings;
        private readonly ILogger logger;

        public CatFactsService(IOptions<ServiceSettings> serviceSettings, ILogger<CatFactsService> logger)
        {
            this.serviceSettings = serviceSettings.Value;
            this.logger = logger;
        }

        public async Task<FactsContainer> GetFactsAsync(int limit)
        {
            var url = $"{serviceSettings.CatFacts}/facts?limit={limit}";
            try
            {
                logger.LogDebug($"Call to :{url}");
                var response = await new HttpClient().GetAsync(url);

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
