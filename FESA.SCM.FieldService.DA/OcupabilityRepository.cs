using FESA.SCM.Common;
using FESA.SCM.FieldService.BE.ReportBE;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService.DA
{
    public class OcupabilityRepository : IOcupabilityRepository
    {
        public IList<Ocupability> GetOcupabilityLevel(string supervisorid, string idsOffice, string idsCostCenter)
        {
            IList<Ocupability> ocupability;
            var database = DatabaseFactory.CreateDatabase();
            using (var cmd = database.GetStoredProcCommand("GET_ALL_PERCENT_STATUS_BY_USER_SP", supervisorid, idsOffice, idsCostCenter))
            {
                using (var reader = database.ExecuteReader(cmd))
                {
                    ocupability = new List<Ocupability>();
                    while (reader.Read())
                    {
                        ocupability.Add(new Ocupability
                        {
                            Percentage = DataConvert.ToDouble(reader["PERCENT"]),
                            UserStatus = (UserStatus)DataConvert.ToInt32(reader["USERSTATUS"])
                        });
                    }
                }
            }
            return ocupability;
        }

    }
}
