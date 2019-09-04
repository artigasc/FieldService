using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService.BE.AssignmentBE
{
    public class Machine
    {
        public string Id { get; set; }
        public string AssignmentId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public int TotalHoursFunction { get; set; }
    }
}
