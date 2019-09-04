using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Models {
    public enum ActivityType {
        PreparingTrip = 0,
        Traveling = 1,
        Driving = 2,
        FieldService = 3,
        StandByClient = 4,
        FieldReport = 5,
        StandByFesa = 6
    }
}
