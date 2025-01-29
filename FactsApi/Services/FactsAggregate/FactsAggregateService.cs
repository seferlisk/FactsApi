using FactsApi.Services.CatFacts;
using FactsApi.Services.DogFacts;
using FactsApi.Services.Interfaces;
using FactsApi.Services.NinjaFacts;
using Microsoft.Extensions.Caching.Memory;

namespace FactsApi.Services.FactsAggregate
{
    /// <summary>
    /// Service for aggregating facts from multiple sources.
    /// This service retrieves facts concurrently from the CatFacts, DogFacts, and NinjaFacts services
    /// and provides options for filtering and limiting the results.
    /// </summary>
    public class FactsAggregateService : IFactsAggregateService
    {
        private readonly ICatFactsService catFactsService;
        private readonly IDogFactsService dogFactsService;
        private readonly INinjaFactsService ninjaFactsService;
        private readonly ILogger<FactsAggregateService> logger;
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="FactsAggregateService"/> class.
        /// </summary>
        /// <param name="catFactsService">Service for retrieving cat facts.</param>
        /// <param name="dogFactsService">Service for retrieving dog facts.</param>
        /// <param name="ninjaFactsService">Service for retrieving ninja facts.</param>
        /// <param name="logger">Logger for capturing application logs and errors.</param>
        /// <param name="memoryCache"></param>
        public FactsAggregateService(
            ICatFactsService catFactsService,
            IDogFactsService dogFactsService,
            INinjaFactsService ninjaFactsService,
            ILogger<FactsAggregateService> logger,
            IMemoryCache memoryCache)
        {
            this.catFactsService = catFactsService;
            this.dogFactsService = dogFactsService;
            this.ninjaFactsService = ninjaFactsService;
            this.logger = logger;
            this.memoryCache = memoryCache;
        }


        /// <summary>
        /// Retrieves a collection of aggregated facts from multiple sources.
        /// </summary>
        /// <param name="limit">The maximum number of facts to retrieve.</param>
        /// <param name="category">The optional category to filter the facts (e.g., "cat", "dog", "ninja").</param>
        /// <returns>A task representing the asynchronous operation, containing a <see cref="FactsContainer"/> with the aggregated facts.</returns>
        /// <remarks>
        /// This method performs the following steps:
        /// 1. Concurrently retrieves facts from the CatFacts, DogFacts, and NinjaFacts services.
        /// 2. Handles errors gracefully for each service by logging the error and adding a fallback fact.
        /// 3. Applies filtering by category, if specified.
        /// 4. Limits the number of facts returned to the specified maximum.
        /// </remarks>
        public async Task<FactsContainer> GetFactsAsync(int limit, string category)
        {
            var cacheKey = $"AggregatedFacts_{limit}_{category}";

            // Check if the data is already cached
            if (memoryCache.TryGetValue(cacheKey, out FactsContainer? cachedFacts))
            {
                logger.LogDebug("Returning aggregated facts from cache.");
                return cachedFacts;
            }

            var facts = new List<Fact>();

            // Concurrently fetch facts with fallbacks
            var tasks = new List<Task>
            {
                FetchWithFallbackAsync(catFactsService.GetFactsAsync, "Cats", limit, facts),
                FetchWithFallbackAsync(dogFactsService.GetFactsAsync, "Dogs", limit, facts),
                FetchWithFallbackAsync(ninjaFactsService.GetFactsAsync, "Ninjas", limit, facts)
            };

            await Task.WhenAll(tasks);

            // Filter by category if provided
            if (!string.IsNullOrEmpty(category))
            {
                facts = facts.Where(f => f.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Limit the number of facts
            facts = facts.Take(limit).ToList();

            var aggregatedFacts = new FactsContainer { Facts = facts };

            // Store the aggregated result in cache with a 5-minute expiration
            memoryCache.Set(cacheKey, aggregatedFacts, TimeSpan.FromMinutes(5));
            logger.LogDebug("Aggregated facts added to cache.");

            return aggregatedFacts;
        }

        /// <summary>
        /// Fetches facts from a single source and adds them to the aggregated facts list.
        /// If the source is unavailable, a fallback fact is added to the list.
        /// </summary>
        /// <param name="fetchMethod">The method to fetch facts from the external API.</param>
        /// <param name="category">The category of the facts (e.g., "Cats").</param>
        /// <param name="limit">The maximum number of facts to retrieve.</param>
        /// <param name="facts">The shared list of aggregated facts.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task FetchWithFallbackAsync(
            Func<int, Task<FactsContainer>> fetchMethod,
            string category,
            int limit,
            List<Fact> facts)
        {
            try
            {
                var result = await fetchMethod(limit);
                if (result?.Facts != null)
                {
                    lock (facts) // Ensure thread safety when adding to the list
                    {
                        facts.AddRange(result.Facts);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to fetch {category} facts: {ex.Message}");
                lock (facts)
                {
                    // Add a fallback fact to maintain context
                    facts.Add(new Fact
                    {
                        Text = $"No {category} facts available at the moment. Please try again later.",
                        Category = category
                    });
                }
            }
        }


    }
}
