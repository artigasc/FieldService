using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FESA.SCM.Web.Models
{
    public class Machine
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public decimal LifeHours { get; set; }
    }
}