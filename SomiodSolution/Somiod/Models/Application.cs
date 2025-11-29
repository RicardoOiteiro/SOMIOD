using System;

namespace Somiod.Models
{
    public class Application
    {
        public string ResourceName { get; set; }
        public string ResType { get; set; } = "application";
        // Use nullable DateTime to map SQL Server datetime2(7); allows NULLs from DB
        public DateTime? CreationDatetime { get; set; }
    }
}