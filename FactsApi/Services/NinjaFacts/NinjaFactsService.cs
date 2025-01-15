using FactsApi.Services.NinjaFacts.DTO;
using FactsApi.Services.NinjaFacts;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FactsApi.Services.NinjaFacts
{
    public class NinjaFactsService : INinjaFactsService
    {
        private readonly ServiceUrls serviceUrls;
        private readonly ILogger logger;

        public NinjaFactsService(IOptions<ServiceUrls> serviceUrls, ILogger<NinjaFactsService> logger)
        {
            this.serviceUrls = serviceUrls.Value;
            this.logger = logger;
        }

        public async Task<List<NinjaFactDTO>> GetNinjaFactsAsync(int limit)
        {
            var url = $"{serviceUrls.NinjaFact}/facts";
            try
            {
                logger.LogDebug($"Call to :{url}");
                var response = await new HttpClient().GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    logger.LogError($"Failed to get ninja facts. Status code: {response.StatusCode}");

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();

                var ninjaFactsResponse = JsonSerializer.Deserialize<List<NinjaFactDTO>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var ninjaFactsResponseWithLimit = ninjaFactsResponse.Take(limit).ToList();

                return ninjaFactsResponseWithLimit;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while fetching ninja facts: {ex.Message}");

                throw;
            }
        }
    }
}
