    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Somiod.Models
{
    public class Subscriptions
    {
        public string ResType { get; set; } = "subscription";
        public string ResourceName { get; set; }
        public string CreationDatetime { get; set; }
        public int Evt { get; set; }
        public string Endpoint { get; set; }
    }
}