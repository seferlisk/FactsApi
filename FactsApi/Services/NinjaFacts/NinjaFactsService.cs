using FactsApi.Services.NinjaFacts.DTO;
using FactsApi.Services.NinjaFacts;
using Microsoft.Extensions.Options;
using System.Text.Json;
using FactsApi.Services.Interfaces;

namespace FactsApi.Services.NinjaFacts
{
    public class NinjaFactsService : INinjaFactsService
    {
        private readonly ServiceSettings serviceSettings;
        private readonly ILogger logger;

        public NinjaFactsService(IOptions<ServiceSettings> serviceSettings, ILogger<NinjaFactsService> logger)
        {
            this.serviceSettings = serviceSettings.Value;
            this.logger = logger;
        }

        public async Task<FactsContainer> GetFactsAsync(int limit)
        {
            var url = $"{serviceSettings.NinjaFacts}/facts";
            try
            {
                logger.LogDebug($"Call to :{url}");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-key", serviceSettings.NinjaFactsApiKey);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    logger.LogError($"Failed to get ninja facts. Status code: {response.StatusCode}");

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();

                var ninjaFactsResponse = JsonSerializer.Deserialize<IEnumerable<NinjaFact>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var ninjaFactsResponseWithLimit = ninjaFactsResponse.Take(limit);

                return new FactsContainer
                {
                    Facts = ninjaFactsResponseWithLimit.Select(s => new Fact
                    {
                        Text = s.Fact,
                        Category = "Ninjas"
                    })
                };

            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while fetching ninja facts: {ex.Message}");

                throw;
            }
        }
    }
}
