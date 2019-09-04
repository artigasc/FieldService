using System.Collections.Generic;
using System.Data;
using System.Linq;
using FESA.SCM.Common;
using FESA.SCM.Identity.BE.UserBE;
using Microsoft.SqlServer.Server;

namespace FESA.SCM.Identity.DA.TableTypes
{
    public class UserTableType : List<User>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new[] {
                    new SqlMetaData("ID", SqlDbType.VarChar, 36),
                    new SqlMetaData("FESAUSERID", SqlDbType.VarChar, 36),
                    new SqlMetaData("NAME", SqlDbType.VarChar, 100),
                    new SqlMetaData("EMAIL", SqlDbType.VarChar, 100),
                    new SqlMetaData("PHONE", SqlDbType.VarChar, 9), 
                    new SqlMetaData("USERNAME", SqlDbType.VarChar, 50),
                    new SqlMetaData("PASSWORD", SqlDbType.VarChar, 200),
                    new SqlMetaData("PHOTO", SqlDbType.VarChar, 200),
                    new SqlMetaData("USERTYPE", SqlDbType.Int),
                    new SqlMetaData("USERSTATUS", SqlDbType.Int),
                    new SqlMetaData("ROLEID", SqlDbType.VarChar, 36),
                    new SqlMetaData("OFFICEID", SqlDbType.Int),
                    new SqlMetaData("COSTCENTERID", SqlDbType.Int),
                    new SqlMetaData("CELULLAR", SqlDbType.VarChar, 50),
                    new SqlMetaData("RPM", SqlDbType.VarChar, 50)
                    }
                );
            foreach (var usrRow in this)
            {
                sdr.SetNullableString(0, usrRow.Id);
                sdr.SetNullableString(1, usrRow.FesaUserId);
                sdr.SetNullableString(2, usrRow.Name);
                sdr.SetNullableString(3, usrRow.Email);
                sdr.SetNullableString(4, usrRow.Phone);
                sdr.SetNullableString(5, usrRow.UserName);
                sdr.SetNullableString(6, usrRow.Password);
                sdr.SetNullableString(7, usrRow.Photo);
                sdr.SetNullableInt32(8, (int) usrRow.UserType);
                sdr.SetNullableInt32(9, (int) usrRow.UserStatus);
                sdr.SetNullableString(10, usrRow.Role.Id);
                sdr.SetNullableInt32(11, usrRow.OfficeId);
                sdr.SetNullableInt32(12, usrRow.CostCenterId);
                sdr.SetNullableString(13, usrRow.Celullar);
                sdr.SetNullableString(14, usrRow.Rpm);
                yield return sdr;
            }
        }
    }
}