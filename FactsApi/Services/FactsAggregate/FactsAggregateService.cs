using FactsApi.Services.CatFacts;
using FactsApi.Services.CatFacts.DTO;
using FactsApi.Services.DogFacts;
using FactsApi.Services.DogFacts.DTO;
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

        public async Task<FactsContainer> GetFactsAsync(int limit)
        {
            var cats = await catFactsService.GetCatFactsAsync(limit);
            var dogs = await dogFactsService.GetDogFactsAsync(limit);
            var ninjas = await ninjaFactsService.GetNinjaFactsAsync(limit);


            //return new FactsContainer
            //{
            //    Facts = cats.Data.Select(x => new Fact { Text = x.Fact, Category = "Cat" }).ToList(),
            //            dogs.Data.Select(x => new Fact { Text = x.Fact, Category = "Dog" }).ToList(),
            //            ninjas.Data.Select(x => new Fact { Text = x.Fact, Category = "Ninja" }).ToList(),
            //};

            //TODO: Error handling (e.g., fallback to default values if an API fails)


            // Aggregate data
            var aggregatedFacts = AggregateFacts(cats, dogs, ninjas);

            // Apply filtering/sorting
            return ApplyFiltersAndSorting(aggregatedFacts, filter);
        }

        private AggregatedResponse AggregateFacts(params object[] data)
        {
            // Combine data into a unified format
            return new AggregatedResponse { Data = data.ToList() };
        }

        private AggregatedResponse ApplyFiltersAndSorting(AggregatedResponse data, AggregationFilter filter)
        {
            // Apply filtering/sorting logic
            return data;
        }
    }
}
