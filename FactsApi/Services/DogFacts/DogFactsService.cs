using FactsApi.Services.DogFacts.DTO;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FactsApi.Services.DogFacts
{
    public class DogFactsService : IDogFactsService
    {
        private readonly ServiceUrls serviceUrls;
        private readonly ILogger logger;

        public DogFactsService(IOptions<ServiceUrls> serviceUrls, ILogger<DogFactsService> logger)
        {
            this.serviceUrls = serviceUrls.Value;
            this.logger = logger;
        }

        public async Task<DogFactsServiceDTO> GetDogFactsAsync(int limit)
        {
            var url = $"{serviceUrls.DogFact}/facts?limit={limit}";
            try
            {
                logger.LogDebug($"Call to :{url}");
                var response = await new HttpClient().GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    logger.LogError($"Failed to get Dog facts. Status code: {response.StatusCode}");

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();

                var dogFactsResponse = JsonSerializer.Deserialize<DogFactsServiceDTO>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return dogFactsResponse;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while fetching dog facts: {ex.Message}");

                throw;
            }
        }
    }          
}
