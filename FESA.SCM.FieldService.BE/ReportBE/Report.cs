using FESA.SCM.Common.Base;
using FESA.SCM.FieldService.BE.AssignmentBE;
using FESA.SCM.FieldService.BE.DocumentBE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService.BE.ReportBE {
	public class Report : Entity {

		public string Id { get; set; }
		public string AssignmentId { get; set; }
		public string Antecedent { get; set; }
		public string Work { get; set; }
		public string Observation { get; set; }
		public string Replacement { get; set; }
		public string Comment1 { get; set; }
		public string Comment2 { get; set; }
		public bool Obs1 { get; set; }
		public bool Obs2 { get; set; }
		public DateTime Date { get; set; }
		public DateTime? Date1 { get; set; }
		public DateTime? Date2 { get; set; }

		public bool Sent1 { get; set; }
		public bool Sent2 { get; set; }

		public string UrlAct { get; set; }
		public string UrlExe { get; set; }

		public string UrlSign { get; set; }
		public string TextReport { get; set; }
		public string UrlFile { get; set; }
		public string ContactId { get; set; }
		public int TotalMinute { get; set; }
		public bool Check { get; set; }
		public List<File> Files { get; set; }
		public int Value { get; set; }
		public bool Online { get; set; }
		public byte[] FileData { get; set; }
		public int ActionType { get; set; }
        public string NameFile { get; set; }
        public int WhoUpdate { get; set; }
    
        public string TotalMinuteStandard1 { get; set; }
        public string TotalMinuteStandard2 { get; set; }
 
	}
}
