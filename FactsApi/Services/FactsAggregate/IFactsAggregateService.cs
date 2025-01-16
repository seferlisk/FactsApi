using FactsApi.Services.DogFacts.DTO;
using FactsApi.Services.Interfaces;

namespace FactsApi.Services.FactsAggregate
{
    /// <summary>
    /// Defines the contract for a service that aggregates facts from multiple sources.
    /// </summary>
    public interface IFactsAggregateService
    {
        /// <summary>
        /// Retrieves a collection of aggregated facts from multiple sources.
        /// </summary>
        /// <param name="limit">The maximum number of facts to retrieve.</param>
        /// <param name="category">
        /// The optional category to filter the facts (e.g., "cats", "dogs", "ninjas").
        /// If null or empty, no filtering by category is applied.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a <see cref="FactsContainer"/>
        /// with the aggregated and optionally filtered facts.
        /// </returns>
        Task<FactsContainer> GetFactsAsync(int limit, string category);
    }
}