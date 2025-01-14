using FactsApi.Services.CatFacts.DTO;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FactsApi.Services.CatFacts
{
    public class CatFactsService : ICatFactsService
    {
        private readonly ServiceUrls serviceUrls;
        private readonly ILogger logger;

        public CatFactsService(IOptions<ServiceUrls> serviceUrls, ILogger<CatFactsService> logger)
        {
            this.serviceUrls = serviceUrls.Value;
            this.logger = logger;
        }

        public async Task<CatFactsServiceDTO> GetCatFactsAsync(int limit)
        {
            var url = $"{serviceUrls.CatFact}/facts?limit={limit}";
            try
            {
                logger.LogDebug($"Call to :{url}");
                var response = await new HttpClient().GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    logger.LogError($"Failed to get cat facts. Status code: {response.StatusCode}");

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();

                var catFactsResponse = JsonSerializer.Deserialize<CatFactsServiceDTO>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});

                return catFactsResponse;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while fetching cat facts: {ex.Message}");

                throw;
            }
        }

    }
}
