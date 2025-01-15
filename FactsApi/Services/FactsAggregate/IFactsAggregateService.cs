using FactsApi.Services.DogFacts.DTO;
using FactsApi.Services.Interfaces;

namespace FactsApi.Services.FactsAggregate
{
    public interface IFactsAggregateService
    {
        Task<FactsContainer> GetFactsAsync(int limit, string category);
    }
}