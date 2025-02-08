using System.Collections.Concurrent;
using System.Diagnostics;

namespace FactsApi
{
    public class ApiStatisticsService
    {
        private readonly ConcurrentDictionary<string, ApiStats> _apiStats = new();

        public void RecordApiCall(string apiName, TimeSpan duration)
        {
            var stats = _apiStats.GetOrAdd(apiName, new ApiStats());

            lock (stats) // Ensure thread safety when updating statistics
            {
                stats.TotalRequests++;
                stats.TotalResponseTime += duration.TotalMilliseconds;
                stats.AverageResponseTime = stats.TotalResponseTime / stats.TotalRequests;

                if (duration.TotalMilliseconds < 100)
                    stats.FastRequests++;
                else if (duration.TotalMilliseconds <= 200)
                    stats.AverageRequests++;
                else
                    stats.SlowRequests++;
            }
        }

        public Dictionary<string, ApiStats> GetStatistics()
        {
            return _apiStats.ToDictionary(entry => entry.Key, entry => entry.Value);
        }
    }

    public class ApiStats
    {
        public int TotalRequests { get; set; }
        public double TotalResponseTime { get; set; } = 0;
        public double AverageResponseTime { get; set; } = 0;
        public int FastRequests { get; set; } = 0;
        public int AverageRequests { get; set; } = 0;
        public int SlowRequests { get; set; } = 0;
    }
}
