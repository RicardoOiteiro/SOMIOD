using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AppArbitro.Dtos
{
    public class ApplicationDto
    {
        [JsonPropertyName("resource-name")]
        public string ResourceName { get; set; }
    }
}
