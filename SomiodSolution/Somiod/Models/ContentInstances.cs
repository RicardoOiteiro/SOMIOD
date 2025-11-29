using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Somiod.Models
{
    public class ContentInstances
    {
        public string ResType { get; set; } = "content-instance";
        public string ResourceName { get; set; }
        public string CreationDatetime { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
    }
}