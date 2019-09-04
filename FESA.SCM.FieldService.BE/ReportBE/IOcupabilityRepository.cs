using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService.BE.ReportBE
{
    public interface IOcupabilityRepository 
    {
        IList<Ocupability> GetOcupabilityLevel(string supervisorid, string idsOffice, string idsCostCenter);
    }
}
