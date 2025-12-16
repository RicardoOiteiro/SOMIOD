using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AppArbitro.Dtos
{
    public class ContainerDto
    {
        [JsonPropertyName("resource-name")]
        public string ResourceName { get; set; }
    }
}
