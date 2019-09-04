using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.Ferreyros.Models
{
    public class ConsultationReport
    {
        public string OT { get; set; }
        public string Client { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Supervisor { get; set; }
        public List<Technical> Technicals { get; set; }
        public string OTStatus { get; set; }
        public string MinutesReport { get; set; }
        public string ExecutiveReport { get; set; }
        public string FinalReport { get; set; }
    }
}
