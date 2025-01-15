namespace FactsApi.Services.CatFacts.DTO
{
    public class CatFactsResponse
    {
        public int CurrentPage { get; set; }
        public IEnumerable<CatFact> Data { get; set; }
        public string FirstPageUrl { get; set; }
        public int From { get; set; }
        public int LastPage { get; set; }
        public string LastPageUrl { get; set; }
        public IEnumerable<CatFactsLink> Links { get; set; }
        public string NextPageUrl { get; set; }
        public string Path { get; set; }
        public int PerPage { get; set; }
        public string PrevPageUrl { get; set; }
        public int To { get; set; }
        public int Total { get; set; }
    }

    public class CatFact
    {
        public string Fact { get; set; }
        public int Length { get; set; }
    }

    public class CatFactsLink
    {
        public string Url { get; set; }
        public string Label { get; set; }
        public bool Active { get; set; }
    }
}
