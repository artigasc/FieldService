using System.Collections.Generic;
using System.Data;
using FESA.SCM.FieldService.BE.ActivityBE;
using Microsoft.SqlServer.Server;

namespace FESA.SCM.FieldService.DA.TableTypes {
    public class ActivityTableType : List<Activity>, IEnumerable<SqlDataRecord> {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator() {
            var sdr = new SqlDataRecord(
                new[]
                {
                    new SqlMetaData("ID", SqlDbType.VarChar, 36),
                    new SqlMetaData("USERID", SqlDbType.VarChar,50),
                    new SqlMetaData("ASSIGNMENTID", SqlDbType.VarChar, 36),
                    new SqlMetaData("ACTIVITYSTATE", SqlDbType.Int),
                    new SqlMetaData("ACTIVITYTYPE", SqlDbType.Int),
                    new SqlMetaData("DESCRIPTION", SqlDbType.VarChar, 200),
                    new SqlMetaData("STARTDATE", SqlDbType.DateTime),
                    new SqlMetaData("ENDDATE", SqlDbType.DateTime),
                    new SqlMetaData("DURATION", SqlDbType.Time),
                    new SqlMetaData("ACTIVE", SqlDbType.Bit), 
					new SqlMetaData("DAY", SqlDbType.Bit),
					new SqlMetaData("ONLINE", SqlDbType.Bit)
				});
            foreach (var activity in this) {
                sdr.SetString(0, activity.Id);
                sdr.SetString(1, activity.UserId);
                sdr.SetString(2, activity.AssignmentId);
                sdr.SetInt32(3, (int)activity.ActivityState);
                sdr.SetInt32(4, (int)activity.ActivityType);
                sdr.SetString(5, activity.Description);
                sdr.SetDateTime(6, activity.StartDate);
				Common.SqlDataRecordExtensions.SetNullableDateTime(sdr, 7, activity.EndDate);
				//sdr.SetDateTime(7, activity.EndDate.HasValue ? activity.EndDate);// ANTONIODATE
				sdr.SetTimeSpan(8, activity.DurationEnd); 
                sdr.SetBoolean(9, activity.Active);
				sdr.SetBoolean(10, activity.Day);
				sdr.SetBoolean(11, activity.Online);
				yield return sdr;
            }

        }
    }
}