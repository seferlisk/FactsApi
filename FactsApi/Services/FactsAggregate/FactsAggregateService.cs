using FactsApi.Services.CatFacts;
using FactsApi.Services.DogFacts;
using FactsApi.Services.DogFacts.DTO;
using FactsApi.Services.Interfaces;
using FactsApi.Services.NinjaFacts;
using FactsApi.Services.NinjaFacts.DTO;

namespace FactsApi.Services.FactsAggregate
{
    public class FactsAggregateService : IFactsAggregateService
    {
        private readonly ICatFactsService catFactsService;
        private readonly IDogFactsService dogFactsService;
        private readonly INinjaFactsService ninjaFactsService;

        public FactsAggregateService(ICatFactsService catFactsService, IDogFactsService dogFactsService, INinjaFactsService ninjaFactsService)
        {
            this.catFactsService = catFactsService;
            this.dogFactsService = dogFactsService;
            this.ninjaFactsService = ninjaFactsService;
        }

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
                facts = facts.Where(f => f.Category.ToString().ToLower() == category.ToLower()).ToList();
            }

            facts = facts.OrderBy(_ => Guid.NewGuid()).Take(limit).ToList();

            return new FactsContainer
            {
                Facts = facts
            };
        }


    }
}
