using Newtonsoft.Json;
using System;

namespace Somiod.Models
{
    public class Application
    {
        // Campo interno usado apenas na BD / SELECTs
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("resource-name")]
        public string ResourceName { get; set; }

        [JsonProperty("res-type")]
        public string ResType { get; set; } = "application";

        [JsonProperty("creation-datetime")]
        public DateTime CreationDatetime { get; set; }

        
    }
}