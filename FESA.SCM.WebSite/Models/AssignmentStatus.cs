using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Models {


    public enum AssignmentStatus {
        Assigned = 1,
        Active = 2,
        Complete = 3,
        StandBy = 4,
        Declined = 9999,
    }
    public static class ConvertAssignmentStatus {
        public static string   GetStatusSPA(object value) {
            switch ((AssignmentStatus)value) {
                case AssignmentStatus.Assigned:
                    return "Asignado";
                case AssignmentStatus.StandBy:
                    return "Stand-by";
                case AssignmentStatus.Active:
                    return "En Proceso";
                case AssignmentStatus.Complete:
                    return "Completado";
                case AssignmentStatus.Declined:
                    return "Rechazado";
                default:
                    break;
            }
            return string.Empty;
        }

    }



}
