using FactsApi.Services.CatFacts.DTO;
using FactsApi.Services.Interfaces;

namespace FactsApi.Services.CatFacts
{
    /// <summary>
    /// Provides methods for retrieving cat facts from an external source.
    /// </summary>
    public interface ICatFactsService: IFactsService
    {
        // No additional members are defined here, but this interface serves as a specialization 
        // of IFactsService specifically for cat facts.
    }
}