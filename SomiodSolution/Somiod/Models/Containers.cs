using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Somiod.Models
{
    public class Containers
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("res-type")]
        public string ResType { get; set; } = "container";

        [JsonProperty("resource-name")]
        public string ResourceName { get; set; }

        [JsonProperty("creation-datetime")]
        public DateTime CreationDatetime { get; set; }
    }
}