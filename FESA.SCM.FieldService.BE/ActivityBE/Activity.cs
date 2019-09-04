using System;
using System.Collections.Generic;
using FESA.SCM.Common.Base;

namespace FESA.SCM.FieldService.BE.ActivityBE {
	public class Activity : Entity {


		public string Group { get; set; }
		public string Name { get; set; }

		public string Id { get; set; }
		public string AssignmentId { get; set; }
		public string UserId { get; set; }
		public ActivityType ActivityType { get; set; }
		public ActivityState ActivityState { get; set; }
		public string Description { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		//public TimeSpan Duration { get; set; }

		public string Duration { get; set; }

		public TimeSpan DurationEnd {
			get {
				return new TimeSpan(0);
			}  //TimeSpan.Parse(Duration); }
		}

		public List<Trace> Traces { get; set; }


		public bool Day { get; set; }
		public bool Online { get; set; }

	}
}