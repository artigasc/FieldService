using System.Collections.Generic;
using System.Data;
using FESA.SCM.FieldService.BE.ActivityBE;
using Microsoft.SqlServer.Server;

namespace FESA.SCM.FieldService.DA.TableTypes {
    public class TraceTableType : List<Trace>, IEnumerable<SqlDataRecord> {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator() {
            var sdr = new SqlDataRecord(
                new[]
                {
                    new SqlMetaData("ID", SqlDbType.VarChar, 36),
                    new SqlMetaData("ACTIVITYID", SqlDbType.VarChar, 36),
                    new SqlMetaData("ACTIVITYSTATE", SqlDbType.Int),
                    new SqlMetaData("LATITUDE", SqlDbType.Float),
                    new SqlMetaData("LONGITUDE", SqlDbType.Float),
                    new SqlMetaData("TRACEDATE", SqlDbType.DateTime),
                }
                );
            foreach (var trace in this) {
                sdr.SetString(0, trace.Id);
                sdr.SetString(1, trace.ActivityId);
                sdr.SetInt32(2, (int)trace.ActivityState);
                sdr.SetDouble(3, trace.Latitude);
                sdr.SetDouble(4, trace.Longitude);
                sdr.SetDateTime(5, trace.TraceDate);
                yield return sdr;
            }
        }
    }
}