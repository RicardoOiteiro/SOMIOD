using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Somiod.Models
{
    public class ContentInstances
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("res-type")]
        public string ResType { get; set; } = "content-instance";

        [JsonProperty("resource-name")]
        public string ResourceName { get; set; }

        [JsonProperty("content-type")]
        public string ContentType { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("creation-datetime")]
        public DateTime CreationDatetime { get; set; }

        // FK para Containers.Id (coluna Container_ID na BD)
        [JsonIgnore]
        public int Container_ID { get; set; }
    }
}