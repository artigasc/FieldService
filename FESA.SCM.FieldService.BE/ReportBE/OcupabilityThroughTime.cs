using FESA.SCM.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService.BE.ReportBE
{
    public class OcupabilityThroughTime : Entity
    {
        public int Quantity { get; set; }
        public string UserId { get; set; }
        public DateTime AssignmentDate { get; set; }
        public UserStatus status { get; set; }
    }
}
