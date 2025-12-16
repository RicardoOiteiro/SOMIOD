using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AppArbitro.Dtos
{
    public class ContentInstanceDto
    {
        [JsonPropertyName("resource-name")]
        public string ResourceName { get; set; }

        [JsonPropertyName("content-type")]
        public string ContentType { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
