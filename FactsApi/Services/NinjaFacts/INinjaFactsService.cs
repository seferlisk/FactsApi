using FactsApi.Services.NinjaFacts.DTO;

namespace FactsApi.Services.NinjaFacts
{
    public interface INinjaFactsService
    {
        Task<List<NinjaFactDTO>> GetNinjaFactsAsync(int limit);
    }
}
