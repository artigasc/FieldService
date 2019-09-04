using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.Ferreyros.Models
{
    public class TechnicalDetail
    {
        public string Activity { get; set; }
        public string Commentary { get; set; }
        public string InitialLocation { get; set; }
        public DateTime DateStart { get; set; }
        public string FinalLocation { get; set; }
        public DateTime DateEnd { get; set; }
        public TimeSpan Duration { get; set; }
        public string ActivityStatus { get; set; }
    }
}
