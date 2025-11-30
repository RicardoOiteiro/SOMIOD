using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Somiod.Models
{
    public class Subscriptions
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("res-type")]
        public string ResType { get; set; } = "subscription";

        [JsonProperty("resource-name")]
        public string ResourceName { get; set; }

        // 1 = creation, 2 = deletion
        [JsonProperty("evt")]
        public int Evt { get; set; }

        [JsonProperty("endpoint")]
        public string Endpoint { get; set; }

        [JsonProperty("creation-datetime")]
        public DateTime CreationDatetime { get; set; }

        // FK para Containers.Id (coluna Container_ID na BD)
        [JsonIgnore]
        public int Container_ID { get; set; }
    }
}