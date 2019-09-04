using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Models {
    public class ReportModel : Entity {
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
        public DateTime? Date { get; set; }
        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public string UrlAct { get; set; }
        public string UrlSign { get; set; }
        public string TextReport { get; set; }
        public string UrlFile { get; set; }
        public ContactModel Contact { get; set; }
        public int TotalMinute { get; set; }
        public bool Check { get; set; }
        public List<FileModel> Files { get; set; }
        public byte[] FileData { get; set; }
        public string UrlExe { get; set; }
        public int ActionType { get; set; }
        public string ContactId { get; set; }
        public string NameFile { get; set; }
        public bool Sent1 { get; set; }
        public bool Sent2 { get; set; }
        public string TotalMinuteStandard1 { get; set; }
        public string TotalMinuteStandard2 { get; set; }
        public int StatusFileReport { get; set; }
        public TimeSpan? DeliveryTime1 { get; set; }
        public TimeSpan? DeliveryTime2 { get; set; }
        public TimeSpan? DelayedTime1 { get; set; }
        public TimeSpan? DelayedTime2 { get; set; }
    }
}
