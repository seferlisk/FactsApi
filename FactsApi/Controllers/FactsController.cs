using FactsApi.Services.FactsAggregate;
using FactsApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FactsApi.Controllers
{
    /// <summary>
    /// Controller for handling requests related to facts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FactsController : ControllerBase
    {
        private readonly ILogger<FactsController> logger;
        private readonly IFactsAggregateService factsAggregateService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FactsController"/> class.
        /// </summary>
        /// <param name="logger">The logger used for logging information and errors (Serilog).</param>
        /// <param name="factsAggregateService">The service responsible for aggregating facts.</param>
        public FactsController(ILogger<FactsController> logger, IFactsAggregateService factsAggregateService)
        {
            this.logger = logger;
            this.factsAggregateService = factsAggregateService;
        }

        /// <summary>
        /// Retrieves a collection of facts.
        /// </summary>
        /// <param name="limit">The maximum number of facts to retrieve. Defaults to 20.</param>
        /// <param name="category">The optional category to filter the facts.</param>
        /// <returns>A task that represents the asynchronous operation, containing a <see cref="FactsContainer"/> with the retrieved facts.</returns>
        /// <remarks>
        /// Example usage:
        /// GET api/Facts?limit=10&category=dog
        /// </remarks>
        [HttpGet(Name = "GetFacts")]
        public async Task<FactsContainer> Get(int limit = 20, string? category = null)
        {
            return await factsAggregateService.GetFactsAsync(limit, category);
        }
    }
}
