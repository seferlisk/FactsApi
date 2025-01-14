using FactsApi.Services.CatFacts.DTO;
using FactsApi.Services.FactsAggregate;
using Microsoft.AspNetCore.Mvc;

namespace FactsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FactsController : ControllerBase
    {
        private readonly ILogger<FactsController> logger;
        private readonly IFactsAggregateService factsAggregateService;

        public FactsController(ILogger<FactsController> logger, IFactsAggregateService factsAggregateService)
        {
            this.logger = logger;
            this.factsAggregateService = factsAggregateService;
        }

        [HttpGet(Name = "GetFacts")]
        public async Task<FactsContainer> Get(int limit = 20)
        {
            return await factsAggregateService.GetFactsAsync(limit);
        }
    }
}
