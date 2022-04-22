using System.Text.Json.Serialization;

namespace PhishingDetector.Models
{
    public class GlobalParameters
    {
        [JsonPropertyName("method")]
        public string Method { get; set; } = "predict";
    }
}
