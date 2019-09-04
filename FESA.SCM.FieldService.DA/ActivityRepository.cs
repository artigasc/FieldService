using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FESA.SCM.Common;
using FESA.SCM.FieldService.BE.ActivityBE;
using FESA.SCM.FieldService.DA.TableTypes;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace FESA.SCM.FieldService.DA {
	public class ActivityRepository : IActivityRepository {
		public int InsertActivities(IList<Activity> activities) {
			var usuarios = new ActivityTableType();
			usuarios.AddRange(activities);
			if (usuarios.Count == 0) return 0;
			var database = DatabaseFactory.CreateDatabase();
			var cmd = database.GetStoredProcCommand("BULK_INSERT_ACTIVITIES_SP");
			cmd.Parameters.Add(new SqlParameter {
				ParameterName = "@ACTIVITIES",
				Value = usuarios,
				DbType = DbType.Object,
				SqlDbType = SqlDbType.Structured,
				TypeName = "ACTIVITY_TYPE"
			});
			return database.ExecuteNonQuery(cmd);
		}

		public IList<Activity> GetActivitiesByAssignmentByUserId(string assignmentId, string userId) {
			IList<Activity> items = null;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ACTIVITIES_BY_ASSIGNMENTID_SP", assignmentId, userId)) {
				using (var reader = database.ExecuteReader(cmd)) {
					items = new List<Activity>();
					while (reader.Read()) {
						items.Add(new Activity {
							Group = DataConvert.ToString(reader["STRGROUP"]),
							Name = DataConvert.ToString(reader["STRNAME"]),
							Id = DataConvert.ToString(reader["ID"]),
							AssignmentId = assignmentId,
							UserId = userId,
							Description = DataConvert.ToString(reader["DESCRIPTION"]),
							ActivityState = (ActivityState)DataConvert.ToInt32(reader["ACTIVITYSTATE"]),
							ActivityType = (ActivityType)DataConvert.ToInt32(reader["ACTIVITYTYPE"]),
							StartDate = DataConvert.ToDateTime(reader["STARTDATE"]),
							EndDate = DataConvert.ToDateTime(reader["ENDDATE"]),
							Duration = DataConvert.ToString(reader["DURATION"]),
							Active = DataConvert.ToBool(reader["Active"]),
							Day = DataConvert.ToBool(reader["BITDAY"]),
							Online = DataConvert.ToBool(reader["BITONLINE"]),
						});
					}
				};
			};
			return items;
		}

		public void InsertTraces(IList<Trace> traces) {
			var usuarios = new TraceTableType();
			usuarios.AddRange(traces);
			if (usuarios.Count == 0) return;
			var database = DatabaseFactory.CreateDatabase();
			var cmd = database.GetStoredProcCommand("BULK_INSERT_TRACE_SP");
			cmd.Parameters.Add(new SqlParameter {
				ParameterName = "@TRACES",
				Value = usuarios,
				DbType = DbType.Object,
				SqlDbType = SqlDbType.Structured,
				TypeName = "TRACE_TYPE"
			});
			database.ExecuteNonQuery(cmd);
		}

		public IList<Trace> GetTracesByActivityId(string activityId) {
			IList<Trace> traces;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("TSP_TRACE_GETBYACTIVITY", activityId)) {
				using (var reader = database.ExecuteReader(cmd)) {
					traces = new List<Trace>();
					while (reader.Read()) {
						traces.Add(new Trace() {
							Id = DataConvert.ToString(reader["ID"]),
							ActivityId = activityId,
							ActivityState = (ActivityState)DataConvert.ToInt32(reader["ACTIVITYSTATE"]),
							TraceDate = DataConvert.ToDateTime(reader["TRACEDATE"]),
							Latitude = DataConvert.ToDouble(reader["LATITUDE"]),
							Longitude = DataConvert.ToDouble(reader["LONGITUDE"])
						});
					}
				}
			}
			return traces;
		}

		public IList<Activity> GetActivitiesByAssignmentId(string assignmentId) {
			IList<Activity> activities = new List<Activity>();
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ACTIVITIES_BY_ONLYASSIGNMENTID_SP", assignmentId)) {
				using (var reader = database.ExecuteReader(cmd)) {
					activities = new List<Activity>();
					while (reader.Read()) {
						activities.Add(new Activity {
							Id = DataConvert.ToString(reader["ID"]),
							AssignmentId = assignmentId,
							Description = DataConvert.ToString(reader["DESCRIPTION"]),
							ActivityState = (ActivityState)DataConvert.ToInt32(reader["ACTIVITYSTATE"]),
							ActivityType = (ActivityType)DataConvert.ToInt32(reader["ACTIVITYTYPE"]),
							StartDate = DataConvert.ToDateTime(reader["STARTDATE"]),
							EndDate = DataConvert.ToDateTime(reader["ENDDATE"]),
							Duration = DataConvert.ToString(reader["DURATION"])
						});
					}
				}
			}
			return activities;
		}


		public IList<ActivityEntity> GetAllEntityActivity() {
			IList<ActivityEntity> items = null;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("TSP_ACTIVITYENTITY_GETALL")) {
				using (var reader = database.ExecuteReader(cmd)) {
					items = new List<ActivityEntity>();
					while (reader.Read()) {
						items.Add(new ActivityEntity {
							Id = DataConvert.ToString(reader["STRID"]),
							Group = DataConvert.ToString(reader["STRGROUP"]),
							Name = DataConvert.ToString(reader["STRNAME"]),
							Value = DataConvert.ToInt32(reader["INTVALUE"]),
							MsgStart = DataConvert.ToString(reader["STRMSGSTART"]),
							MsgEnd = DataConvert.ToString(reader["STRMSGEND"]),
							App = DataConvert.ToBool(reader["BITAPP"]),
							Visible = DataConvert.ToBool(reader["BITVISIBLE"])
						});
					}
				}
			}
			return items;
		}



	}
}