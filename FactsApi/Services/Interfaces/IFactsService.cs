namespace FactsApi.Services.Interfaces
{
    public interface IFactsService
    {
        Task<FactsContainer> GetFactsAsync(int limit);
    }
}
