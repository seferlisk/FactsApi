using FactsApi.Services.CatFacts;
using FactsApi.Services.CatFacts.DTO;
using FactsApi.Services.DogFacts;
using FactsApi.Services.DogFacts.DTO;

namespace FactsApi.Services.FactsAggregate
{
    public class FactsAggregateService : IFactsAggregateService
    {
        private readonly ICatFactsService catFactsService;
        private readonly IDogFactsService dogFactsService;

        public FactsAggregateService(ICatFactsService catFactsService, IDogFactsService dogFactsService)
        {
            this.catFactsService = catFactsService;
            this.dogFactsService = dogFactsService;
        }

        public async Task<FactsContainer> GetFactsAsync(int limit)
        {
            var cats = await catFactsService.GetCatFactsAsync(limit);
            var dogs = await dogFactsService.GetDogFactsAsync(limit);


            return new FactsContainer
            {
                Facts = cats.Data.Select(x => new Fact { Text = x.Fact, Category = "Cat" }).ToList(),
                        dogs.Data.Select(x => new Fact { Text = x.Fact, Category = "Dog" }).ToList()
            };

        }
    }
}
