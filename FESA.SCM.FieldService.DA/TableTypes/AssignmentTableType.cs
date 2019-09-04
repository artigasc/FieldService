using System.Collections.Generic;
using System.Data;
using FESA.SCM.FieldService.BE.AssignmentBE;
using Microsoft.SqlServer.Server;

namespace FESA.SCM.FieldService.DA.TableTypes {
    public class AssignmentTableType : List<Assignment>, IEnumerable<SqlDataRecord> {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator() {
            var sdr = new SqlDataRecord(
                new[]
                {
                    new SqlMetaData("ID", SqlDbType.VarChar, 36),
                    //new SqlMetaData("WORKORDERID", SqlDbType.VarChar, 36),
                    //new SqlMetaData("LOCATIONID", SqlDbType.VarChar, 6),
                    new SqlMetaData("STARTDATE", SqlDbType.DateTime),
                    new SqlMetaData("ENDDATE", SqlDbType.DateTime),
                    new SqlMetaData("ASSIGNMENTTYPE", SqlDbType.Int),
                    new SqlMetaData("ASSIGNMENTSTATUS", SqlDbType.Int),                    
                    //new SqlMetaData("DESCRIPTION", SqlDbType.VarChar, 200),
                    //new SqlMetaData("PRIORITY", SqlDbType.Int),
                    //new SqlMetaData("REGISTERDATE", SqlDbType.DateTime),
                    //new SqlMetaData("ESTIMATEDSTARTDATE", SqlDbType.DateTime),
                    //new SqlMetaData("ESTIMATEDENDDATE", SqlDbType.DateTime),
                    
                });
            foreach (var activity in this) {
                sdr.SetString(0, activity.Id);
                sdr.SetDateTime(1, activity.StartDate);
                sdr.SetDateTime(2, activity.EndDate);
                sdr.SetInt32(3, (int)activity.AssignmentType);
                sdr.SetInt32(4, (int)activity.Status);
                //sdr.SetString(1, activity.WorkOrderId);
                //sdr.SetString(2, activity.Location.Id);
                //sdr.SetString(5, activity.Description);
                //sdr.SetDateTime(6, activity.RequestDate);
                //sdr.SetDateTime(7, activity.EstimatedStartDate);
                //sdr.SetDateTime(8, activity.EstimatedEndDate);
                yield return sdr;
            }
        }
    }
}