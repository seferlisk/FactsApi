using FactsApi.Services.DogFacts.DTO;
using FactsApi.Services.Interfaces;

namespace FactsApi.Services.DogFacts
{
    /// <summary>
    /// Retrieves one dog fact.
    /// </summary>
    /// <param name="limit">The maximum number of dog facts to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a <see cref="FactsContainer"/>
    /// with the retrieved dog facts.
    /// </returns>
    public interface IDogFactsService : IFactsService
    {
        // No additional members are defined here, but this interface serves as a specialization 
        // of IFactsService specifically for dog facts.
    }
}

