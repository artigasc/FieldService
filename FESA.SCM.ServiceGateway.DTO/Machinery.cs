using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.ServiceGateway.DTO
{
    public class Machinery
    {
        public string Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public decimal LifeHours { get; set; }
        public int TotalHoursFunction { get; set; }
    }
}
