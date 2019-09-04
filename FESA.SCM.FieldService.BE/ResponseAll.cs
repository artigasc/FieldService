using FESA.SCM.FieldService.BE.ActivityBE;
using FESA.SCM.FieldService.BE.AssignmentBE;
using FESA.SCM.FieldService.BE.DocumentBE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService.BE {
	public class ResponseAll {


		public Assignment Assignment { get; set; }
		public List<Assignment> Assignments { get; set; }
		public List<ActivityEntity> ActivitiesEntity { get; set; }
		public List<DocumentEntity> DocumentsEntity { get; set; }

		public string Message { get; set; }
		public int TotalRows { get; set; }

	}
}
