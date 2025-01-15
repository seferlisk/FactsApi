using FactsApi.Services.DogFacts.DTO;

namespace FactsApi.Services.DogFacts
{
    public interface IDogFactsService
    {
        Task<DogsFactsDTO> GetDogFactsAsync(int limit);
    }
}

