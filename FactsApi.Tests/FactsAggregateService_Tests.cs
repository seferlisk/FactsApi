using FactsApi.Services.CatFacts;
using FactsApi.Services.DogFacts;
using FactsApi.Services.FactsAggregate;
using FactsApi.Services.Interfaces;
using FactsApi.Services.NinjaFacts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FactsApi.Tests
{
    public class FactsAggregateService_Tests
    {
        [Fact]
        public async Task GetFactsAsync_ReturnsAggregatedFactsWithFallback()
        {
            // Arrange
            var catFactsService = new Mock<ICatFactsService>();
            var dogFactsService = new Mock<IDogFactsService>();
            var ninjaFactsService = new Mock<INinjaFactsService>(); // Simulate failure

            var logger = new Mock<ILogger<FactsAggregateService>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            //  Simulate Cat API returning valid data
            catFactsService.Setup(s => s.GetFactsAsync(It.IsAny<int>()))
                .ReturnsAsync(new FactsContainer
                {
                    Facts = new List<Fact> { new Fact { Text = "Cat fact 1", Category = "Cats" } }
                });

            //  Simulate Dog API returning valid data
            dogFactsService.Setup(s => s.GetFactsAsync(It.IsAny<int>()))
                .ReturnsAsync(new FactsContainer
                {
                    Facts = new List<Fact> { new Fact { Text = "Dog fact 1", Category = "Dogs" } }
                });

            //  Simulate Ninja API **throwing an exception** (Failure scenario)
            ninjaFactsService.Setup(s => s.GetFactsAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Ninja API is down"));

            var service = new FactsAggregateService(
                catFactsService.Object, dogFactsService.Object, ninjaFactsService.Object, logger.Object, memoryCache);

            //  Pass "Ninjas" as the category to check if the fallback message is properly added
            var result = await service.GetFactsAsync(10, "Ninjas");

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Facts);
            Assert.Equal(3, result.Facts.Count()); // Expecting 3 facts: 1 cat, 1 dog, 1 fallback

            //  Ensure valid categories exist
            Assert.Contains(result.Facts, f => f.Category == "Cats");
            Assert.Contains(result.Facts, f => f.Category == "Dogs");

            //  Ensure a fallback message exists for the failed API (Ninja)
            Assert.Contains(result.Facts, f => f.Category == "Ninjas" && f.Text.Contains("No Ninjas facts available"));
        }

        [Fact]
        public async Task GetFactsAsync_ReturnsLimitedAggregatedFacts()
        {
            // Arrange
            var catFactsService = new MockCatFactsService();
            var dogFactsService = new MockDogFactsService();
            var ninjaFactsService = new MockNinjaFactsService();

            var logger = new MockLogger<FactsAggregateService>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var service = new FactsAggregateService(catFactsService, dogFactsService, ninjaFactsService, logger, memoryCache);

            // Act
            var result = await service.GetFactsAsync(2, null);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Facts);
            Assert.Equal(2, result.Facts.Count());
        }

        [Fact]
        public async Task GetFactsAsync_FiltersByCategory()
        {
            // Arrange
            var catFactsService = new MockCatFactsService();
            var dogFactsService = new MockDogFactsService();
            var ninjaFactsService = new MockNinjaFactsService();

            var logger = new MockLogger<FactsAggregateService>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var service = new FactsAggregateService(catFactsService, dogFactsService, ninjaFactsService, logger, memoryCache);

            // Act
            var result = await service.GetFactsAsync(10, "cat");

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Facts);
            Assert.All(result.Facts, fact => Assert.Equal("cat", fact.Category, ignoreCase: true));
        }

        // Mock services for testing
        private class MockCatFactsService : ICatFactsService
        {
            public Task<FactsContainer> GetFactsAsync(int limit)
            {
                return Task.FromResult(new FactsContainer
                {
                    Facts = new List<Fact>
                    {
                        new Fact { Text = "Cat fact 1", Category = "cat" }
                    }
                });
            }
        }

        private class MockDogFactsService : IDogFactsService
        {
            public Task<FactsContainer> GetFactsAsync(int limit)
            {
                return Task.FromResult(new FactsContainer
                {
                    Facts = new List<Fact>
                    {
                        new Fact { Text = "Dog fact 1", Category = "dog" }
                    }
                });
            }
        }

        private class MockNinjaFactsService : INinjaFactsService
        {
            public Task<FactsContainer> GetFactsAsync(int limit)
            {
                return Task.FromResult(new FactsContainer
                {
                    Facts = new List<Fact>
                    {
                        new Fact { Text = "Ninja fact 1", Category = "ninja" }
                    }
                });
            }
        }

        private class FailingNinjaFactsService : INinjaFactsService
        {
            public Task<FactsContainer> GetFactsAsync(int limit)
            {
                throw new Exception("NinjaFacts API is unavailable");
            }
        }

        private class MockLogger<T> : ILogger<T>
        {
            public IDisposable BeginScope<TState>(TState state) => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                // You can leave this method empty or log to the console for debugging during tests.
                Console.WriteLine(formatter(state, exception));
            }
        }
    }
}
