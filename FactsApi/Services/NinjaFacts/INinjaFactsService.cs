using FactsApi.Services.NinjaFacts.DTO;

namespace FactsApi.Services.NinjaFacts
{
    public interface INinjaFactsService
    {
        Task<NinjaFactsServiceDTO> GetNinjaFactsAsync(int limit);
    }
}
