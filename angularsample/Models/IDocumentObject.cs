using Newtonsoft.Json;

namespace AngularPerformanceSamples.Models
{
    public interface IDocumentObject
    {
        [JsonProperty(PropertyName = "id")]
        string Id { get; set; }
        string Data { get; set; }
    }
}
