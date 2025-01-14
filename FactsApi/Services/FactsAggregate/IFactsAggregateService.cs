using FactsApi.Services.CatFacts.DTO;

namespace FactsApi.Services.FactsAggregate
{
    public interface IFactsAggregateService
    {
        Task<FactsContainer> GetFactsAsync(int limit);
    }
}