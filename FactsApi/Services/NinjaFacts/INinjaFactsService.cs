using FactsApi.Services.Interfaces;
using FactsApi.Services.NinjaFacts.DTO;

namespace FactsApi.Services.NinjaFacts
{
    /// <summary>
    /// Defines the contract for a service that provides facts about ninjas.
    /// </summary>
    public interface INinjaFactsService : IFactsService
    {
        // No additional members are defined here, but this interface serves as a specialization 
        // of IFactsService specifically for ninja facts.
    }
}
