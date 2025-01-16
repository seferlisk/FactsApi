namespace FactsApi.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for a service that retrieves facts from an external source.
    /// </summary>
    public interface IFactsService
    {
        /// <summary>
        /// Retrieves a collection of facts.
        /// </summary>
        /// <param name="limit">The maximum number of facts to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a <see cref="FactsContainer"/>
        /// with the retrieved facts.
        /// </returns>
        Task<FactsContainer> GetFactsAsync(int limit);
    }
}
