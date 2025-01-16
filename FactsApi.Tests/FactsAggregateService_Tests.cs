using FactsApi.Services.CatFacts;
using FactsApi.Services.DogFacts;
using FactsApi.Services.FactsAggregate;
using FactsApi.Services.Interfaces;
using FactsApi.Services.NinjaFacts;
using Xunit;

namespace FactsApi.Tests
{
    public class FactsAggregateService_Tests
    {
        [Fact]
        public async Task GetFactsAsync_ReturnsAggregatedFacts()
        {
            // Arrange
            var catFactsService = new MockCatFactsService();
            var dogFactsService = new MockDogFactsService();
            var ninjaFactsService = new MockNinjaFactsService();

            var service = new FactsAggregateService(catFactsService, dogFactsService, ninjaFactsService);

            // Act
            var result = await service.GetFactsAsync(10, null);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Facts);
            Assert.Equal(3, result.Facts.Count());
        }

        [Fact]
        public async Task GetFactsAsync_ReturnsLimitedAggregatedFacts()
        {
            // Arrange
            var catFactsService = new MockCatFactsService();
            var dogFactsService = new MockDogFactsService();
            var ninjaFactsService = new MockNinjaFactsService();

            var service = new FactsAggregateService(catFactsService, dogFactsService, ninjaFactsService);

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

            var service = new FactsAggregateService(catFactsService, dogFactsService, ninjaFactsService);

            // Act
            var result = await service.GetFactsAsync(10, "cat");

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Facts);
            Assert.All(result.Facts, fact => Assert.Equal("cat", fact.Category, ignoreCase: true));
        }

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
    }
}
