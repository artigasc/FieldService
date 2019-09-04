using FESA.SCM.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService.BE.ReportBE
{
    public class Ocupability : Entity
    {
        public double Percentage { get; set; }
        public UserStatus UserStatus { get; set; }
    }
}
