using FESA.SCM.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService.BE.ActivityBE {
	public class ActivityEntity : Entity {

		public string Id { get; set; }
		public string Group { get; set; }
		public string Name { get; set; }
		public int Value { get; set; }
		public string MsgStart { get; set; }
		public string MsgEnd { get; set; }
		public bool App { get; set; }
		public bool Visible { get; set; }
		public Activity Activity { get; set; }


	}
}
