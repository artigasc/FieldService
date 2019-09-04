using FESA.SCM.FieldService.BE.ReportBE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService.BL.BusinessInterfaces
{
    public interface IOcupabilityThroughTimeService
    {
        IList<OcupabilityThroughTime> GetOcupabilityThroughTime(string supervisorId, int status);
       
    }
}

