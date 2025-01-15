using FactsApi.Services.CatFacts.DTO;
using FactsApi.Services.DogFacts.DTO;

namespace FactsApi.Services.FactsAggregate
{
    public interface IFactsAggregateService
    {
        Task<FactsContainer> GetFactsAsync(int limit);
    }
}