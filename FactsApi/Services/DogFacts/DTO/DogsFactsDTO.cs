using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FactsApi.Services.DogFacts.DTO
{
    public class DogsFactsDTO
    {
        public List<DogFact> Data { get; set; }
    }

    public  class DogFact
    {        
        public Guid Id { get; set; }
        
        public string Type { get; set; }
        
        public DogFactAttributes Attributes { get; set; }
    }

    public  class DogFactAttributes
    {        
        public string Body { get; set; }
    }
}
