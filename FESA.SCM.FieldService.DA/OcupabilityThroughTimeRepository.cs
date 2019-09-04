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
    public class OcupabilityThroughTimeRepository : IOcupabilityThroughTimeRepository
    {
        public IList<OcupabilityThroughTime> GetOcupabilityThroughTime(string supervisorId, int status)
        {
            IList<OcupabilityThroughTime> ocupability;
            var database = DatabaseFactory.CreateDatabase();
            using (var cmd = database.GetStoredProcCommand("GET_USERS_DETAILS_SP", supervisorId, status))
            {
                using (var reader = database.ExecuteReader(cmd))
                {
                    ocupability = new List<OcupabilityThroughTime>();
                    while (reader.Read())
                    {
                        ocupability.Add(new OcupabilityThroughTime
                        {
                            Quantity = DataConvert.ToInt32(reader["QUANTITY"]),
                            status = (UserStatus)DataConvert.ToInt32(reader["USERSTATUS"]),
                            AssignmentDate = DataConvert.ToDateTime(reader["DATE"])
                        });
                    }
                }
            }
            return ocupability;
        }

    }
}
