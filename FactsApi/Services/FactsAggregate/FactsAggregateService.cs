using FactsApi.Services.CatFacts;
using FactsApi.Services.DogFacts;
using FactsApi.Services.DogFacts.DTO;
using FactsApi.Services.Interfaces;
using FactsApi.Services.NinjaFacts;
using FactsApi.Services.NinjaFacts.DTO;

namespace FactsApi.Services.FactsAggregate
{
    /// <summary>
    /// Service for aggregating facts from multiple sources.
    /// </summary>
    public class FactsAggregateService : IFactsAggregateService
    {
        private readonly ICatFactsService catFactsService;
        private readonly IDogFactsService dogFactsService;
        private readonly INinjaFactsService ninjaFactsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FactsAggregateService"/> class.
        /// </summary>
        /// <param name="catFactsService">Service for retrieving cat facts.</param>
        /// <param name="dogFactsService">Service for retrieving dog facts.</param>
        /// <param name="ninjaFactsService">Service for retrieving ninja facts.</param>
        public FactsAggregateService(ICatFactsService catFactsService, IDogFactsService dogFactsService, INinjaFactsService ninjaFactsService)
        {
            this.catFactsService = catFactsService;
            this.dogFactsService = dogFactsService;
            this.ninjaFactsService = ninjaFactsService;
        }

        /// <summary>
        /// Retrieves a collection of aggregated facts from multiple sources.
        /// </summary>
        /// <param name="limit">The maximum number of facts to retrieve.</param>
        /// <param name="category">The optional category to filter the facts (e.g., "cat", "dog", "ninja").</param>
        /// <returns>A task representing the asynchronous operation, containing a <see cref="FactsContainer"/> with the aggregated facts.</returns>
        /// <remarks>
        /// This method retrieves facts from the CatFacts, DogFacts, and NinjaFacts services concurrently.
        /// It supports filtering by category and limits the total number of facts.
        /// </remarks>
        public async Task<FactsContainer> GetFactsAsync(int limit, string category)
        {
            var catsTask = catFactsService.GetFactsAsync(limit);
            var dogsTask = dogFactsService.GetFactsAsync(limit);
            var ninjasTask = ninjaFactsService.GetFactsAsync(limit);

            var allResults = await Task.WhenAll(catsTask, dogsTask, ninjasTask);

            var facts = new List<Fact>();

            foreach (var result in allResults)
            {
                facts.AddRange(result.Facts);
            }

            if (!string.IsNullOrEmpty(category))
            {
                facts = facts.Where(f => f.Category.ToLower() == category.ToLower()).ToList();
            }

            facts = facts.OrderBy(_ => Guid.NewGuid()).Take(limit).ToList();

            return new FactsContainer
            {
                Facts = facts
            };
        }


    }
}
