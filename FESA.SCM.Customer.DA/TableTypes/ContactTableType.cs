using System.Collections.Generic;
using System.Data;
using FESA.SCM.Common;
using FESA.SCM.Customer.BE.ContactBE;
using Microsoft.SqlServer.Server;

namespace FESA.SCM.Customer.DA.TableTypes
{
    public class ContactTableType : List<Contact>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new[]
                {
                    new SqlMetaData("ID", SqlDbType.VarChar, 36),
                    new SqlMetaData("CUSTOMERID", SqlDbType.VarChar, 36),
                    new SqlMetaData("NAME", SqlDbType.VarChar, 50),
                    new SqlMetaData("LASTNAME", SqlDbType.VarChar, 50),
                    new SqlMetaData("EMAIL", SqlDbType.VarChar, 20),
                    new SqlMetaData("CHARGE", SqlDbType.VarChar, 20),
                    new SqlMetaData("PHONE", SqlDbType.VarChar, 10),
                }
                );
            foreach (var contactRow in this)
            {
                sdr.SetNullableString(0, contactRow.Id);
                sdr.SetNullableString(1, contactRow.CustomerId);
                sdr.SetNullableString(2, contactRow.Name);
                sdr.SetNullableString(3, contactRow.LastName);
                sdr.SetNullableString(4, contactRow.Email);
                sdr.SetNullableString(5, contactRow.Charge);
                sdr.SetNullableString(6, contactRow.Phone);
                yield return sdr;
            }
        }
    }
}