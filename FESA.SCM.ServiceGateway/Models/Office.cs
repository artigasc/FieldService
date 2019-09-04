using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FESA.SCM.ServiceGateway.Models {
	public class Office {
		public int Id { get; set; }
		public string Description { get; set; }
		public string City { get; set; }
		public string StrOffice { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
	}
}