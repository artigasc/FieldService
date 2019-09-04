using FESA.SCM.Identity.BE.UserBE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.Identity.BE {
	public class ResponseFilter {

		public List<CostCenter> CostsCenter { get; set; }
		public List<Office> Offices { get; set; }
		public List<Brand> Brands { get; set; }
	}
}
