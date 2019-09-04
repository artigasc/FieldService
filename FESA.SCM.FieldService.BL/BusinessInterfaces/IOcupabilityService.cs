using FESA.SCM.FieldService.BE.ReportBE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService.BL.BusinessInterfaces
{
    public interface IOcupabilityService
    {
        IList<Ocupability> GetOcupabilityLevel(string supervisorId, string idsOffice, string idsCostCenter);
    }
}
