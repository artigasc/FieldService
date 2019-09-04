using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.ServiceGateway.DTO {
	public class ResponseFilter {

		public List<dynamic> CostsCenter { get; set; }
		public List<dynamic> Offices { get; set; }
		public List<dynamic> Brands { get; set; }

	}
}
