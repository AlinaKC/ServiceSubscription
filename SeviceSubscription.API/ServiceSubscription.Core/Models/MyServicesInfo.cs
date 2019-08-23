using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceSubscription.Core
{
    public class MyServicesInfo
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string ServiceTypeName { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
