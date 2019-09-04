using FESA.SCM.ServiceGateway.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FESA.SCM.ServiceGateway.Models {

	public class ResponseAll {

		public Assignment Assignment { get; set; }

		public List<Assignment> Assignments { get; set; }
		public List<dynamic> ActivitiesEntity { get; set; }
		public List<dynamic> DocumentsEntity { get; set; }
		public string Message { get; set; }

		public int TotalRows { get; set; }

	}

}