using FactsApi.Services.CatFacts.DTO;

namespace FactsApi.Services.CatFacts
{
    public interface ICatFactsService
    {
        Task<CatFactsServiceDTO> GetCatFactsAsync(int limit);
    }
}