using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AppArbitro.Dtos
{
    public class SubscriptionDto
    {
        [JsonPropertyName("resource-name")]
        public string ResourceName { get; set; }

        // 1 = criar, 2 = eliminar
        [JsonPropertyName("evt")]
        public int Evt { get; set; }

        
        [JsonPropertyName("endpoint")]
        public string Endpoint { get; set; }
    }
}
