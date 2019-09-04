using FESA.SCM.FieldService.BE.ReportBE;
using FESA.SCM.FieldService.BL.BusinessInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService.BL.BusinessServices
{
    public class OcupabilityService : IOcupabilityService
    {
        #region Members
        private readonly IOcupabilityRepository _ocupabilityRepository;
        #endregion

        #region Constructor
        public OcupabilityService(IOcupabilityRepository ocupabilityRepository)
        {
            if (ocupabilityRepository == null)
                throw new ArgumentNullException(nameof(ocupabilityRepository));

            _ocupabilityRepository = ocupabilityRepository;
        }
        #endregion

        #region Methods
        public IList<Ocupability> GetOcupabilityLevel(string supervisorid, string idsOffice, string idsCostCenter) {
            return _ocupabilityRepository.GetOcupabilityLevel(supervisorid, idsOffice, idsCostCenter);
        }
        #endregion
    }
}
