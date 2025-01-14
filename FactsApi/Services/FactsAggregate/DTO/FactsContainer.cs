namespace FactsApi.Services.CatFacts.DTO
{
    public class FactsContainer
    {
        public List<Fact> Facts { get; set; }
    }

    public class Fact
    {
        public string Text { get; set; }
        public string Category { get; set; }
    }
}
