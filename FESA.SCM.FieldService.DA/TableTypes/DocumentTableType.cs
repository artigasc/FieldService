using System.Collections.Generic;
using System.Data;
using FESA.SCM.Common;
using FESA.SCM.FieldService.BE.DocumentBE;
using Microsoft.SqlServer.Server;

namespace FESA.SCM.FieldService.DA.TableTypes {
	public class DocumentTableType : List<Document>, IEnumerable<SqlDataRecord> {
		IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator() {
			var sdr = new SqlDataRecord(
				new[]
				{
					new SqlMetaData("STRID", SqlDbType.NVarChar, 36),
					new SqlMetaData("STRIDASSIGNMENT", SqlDbType.VarChar, 36),
					new SqlMetaData("STRIDUSER", SqlDbType.VarChar, 36),
					new SqlMetaData("STRIDDOCUMENT", SqlDbType.NVarChar, 36),
					new SqlMetaData("STRIDACTIVITY", SqlDbType.NVarChar, 36),
					new SqlMetaData("INTVALUE", SqlDbType.Int),
					new SqlMetaData("STRTEXT", SqlDbType.NText),
					new SqlMetaData("DTTDATE", SqlDbType.DateTime),
					new SqlMetaData("STRCREATEDBY", SqlDbType.NVarChar, 100),
				}
				);
			foreach (var usrRow in this) {
				sdr.SetString(0, usrRow.Id);
				sdr.SetString(1, usrRow.AssignmentId);
				sdr.SetString(2, usrRow.UserId);
				sdr.SetString(3, usrRow.DocumentId);
				sdr.SetString(4, usrRow.ActivityId);
				sdr.SetInt32(5, usrRow.ActivityValue);
				SqlDataRecordExtensions.SetNullableString(sdr, 6, usrRow.Text);
				//sdr.SetString(6, usrRow.Text);
				sdr.SetDateTime(7, usrRow.Date);
				sdr.SetString(8, usrRow.CreatedBy);
				yield return sdr;
			}
		}
	}
}