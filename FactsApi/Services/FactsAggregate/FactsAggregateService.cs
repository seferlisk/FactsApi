using FactsApi.Services.CatFacts;
using FactsApi.Services.CatFacts.DTO;

namespace FactsApi.Services.FactsAggregate
{
    public class FactsAggregateService : IFactsAggregateService
    {
        private readonly ICatFactsService catFactsService;

        public FactsAggregateService(ICatFactsService catFactsService)
        {
            this.catFactsService = catFactsService;
        }

        public async Task<FactsContainer> GetFactsAsync(int limit)
        {
            var cats = await catFactsService.GetCatFactsAsync(limit);


            return new FactsContainer
            {
                Facts = cats.Data.Select(x => new Fact { Text = x.Fact, Category = "Cat" }).ToList()
            };

        }
    }
}
