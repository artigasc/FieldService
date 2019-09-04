using FESA.SCM.FieldService.BL.BusinessInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FESA.SCM.FieldService.BE.ReportBE;

namespace FESA.SCM.FieldService.BL.BusinessServices
{
    public class OcupabilityThroughTimeService : IOcupabilityThroughTimeService
    {
        private readonly IOcupabilityThroughTimeRepository _ocupabilityThroughTimeRepository;

        public OcupabilityThroughTimeService(IOcupabilityThroughTimeRepository ocupabilityThroughTimeRepository)
        {
            _ocupabilityThroughTimeRepository = ocupabilityThroughTimeRepository;
            if (_ocupabilityThroughTimeRepository == null)
                throw new ArgumentNullException(nameof(_ocupabilityThroughTimeRepository));

            
        }
        public IList<OcupabilityThroughTime> GetOcupabilityThroughTime(string supervisorId, int status)
        {
            return _ocupabilityThroughTimeRepository.GetOcupabilityThroughTime(supervisorId, status);
        }
    }
}
