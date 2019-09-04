using FESA.SCM.FieldService.BE.ActivityBE;
using FESA.SCM.FieldService.BE.AssignmentBE;
using FESA.SCM.FieldService.BE.DocumentBE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService.BE {
	public class Request {

		public Assignment Assignment { get; set; }
		public List<Document> Documents { get; set; }
		public List<Activity> Activities { get; set; }

		public List<ReportBE.Report> Reports { get; set; }

		public int PageIndex { get; set; }
		public int PageSize { get; set; }


	}
}
