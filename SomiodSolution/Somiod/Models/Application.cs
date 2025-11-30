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
        // Use nullable DateTime to map SQL Server datetime2(7); allows NULLs from DB
        [JsonProperty("creation-datetime")]
        public DateTime CreationDatetime { get; set; }

        // FK para Application.Id (coluna Application_ID na BD)
        [JsonIgnore]
        public int Application_ID { get; set; }
    }
}