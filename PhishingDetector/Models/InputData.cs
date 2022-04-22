using System.Text.Json.Serialization;

namespace PhishingDetector.Models
{
    public class InputData
    {
        [JsonPropertyName("data")]
        public List<UrlData> Data { get; set; }
    }
}
