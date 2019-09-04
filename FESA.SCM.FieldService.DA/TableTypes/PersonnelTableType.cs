using System.Collections.Generic;
using System.Data;
using FESA.SCM.FieldService.BE.AssignmentBE;
using Microsoft.SqlServer.Server;

namespace FESA.SCM.FieldService.DA.TableTypes
{
    public class PersonnelTableType : List<Personnel>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new[]
                {
                    new SqlMetaData("ID", SqlDbType.VarChar, 36),
                    new SqlMetaData("ASSINGMENTID", SqlDbType.VarChar, 36),
                    new SqlMetaData("PERSONNELTYPE", SqlDbType.Int),
                }
                );
            foreach (var person in this)
            {
                sdr.SetString(0, person.Id);
                sdr.SetString(1, person.AssignmentId);
                sdr.SetInt32(2, (int) person.PersonnelType);
                yield return sdr;
            }
        }
    }
}