using System.Collections.Generic;

namespace FESA.SCM.FieldService.BE.ActivityBE {
	public interface IActivityRepository {
		int InsertActivities(IList<Activity> activities);
		void InsertTraces(IList<Trace> traces);
		IList<Trace> GetTracesByActivityId(string activityId);
		IList<Activity> GetActivitiesByAssignmentId(string assignmentid);


		//NEW
		IList<ActivityEntity> GetAllEntityActivity();

		//MODIFY
		IList<Activity> GetActivitiesByAssignmentByUserId(string assignmentId, string userId);

		//DELETE
		//IList<Activity> GetActivitiesByAssignmentByUserId(string assignmentId, string userId);


	}
}