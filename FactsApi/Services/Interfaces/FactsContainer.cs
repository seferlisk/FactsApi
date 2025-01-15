namespace FactsApi.Services.Interfaces
{
    public class FactsContainer
    {
        public IEnumerable<Fact>? Facts { get; set; }
    }

    public class Fact
    {
        public string Text { get; set; }
        public string Category { get; set; }
    }

}
