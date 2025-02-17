using Microsoft.AspNetCore.Mvc;

namespace FactsApi.Controllers
{
    [ApiController]
    [Route("api/stats")]
    public class StatsController : ControllerBase
    {
        private readonly ApiStatisticsService _apiStatisticsService;

        public StatsController(ApiStatisticsService apiStatisticsService)
        {
            _apiStatisticsService = apiStatisticsService;
        }

        [HttpGet]
        public IActionResult GetStatistics()
        {
            var stats = _apiStatisticsService.GetStatistics();
            return Ok(stats);
        }
    }
}
