using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Models
{
    public enum UserType {
        Technician = 0,
        Supervisor = 1,
        Lead = 2
    }

    public static class ConvertUserType {
        public static string GetStatusSPA(object value) {
            switch ((UserType)value) {
                case UserType.Technician:
                    return "Técnico";
                case UserType.Supervisor:
                    return "Supervisor";
                case UserType.Lead:
                    return "Lider";
                default:
                    break;
            }
            return string.Empty;
        }

        public static int GetStatusUserInt(object value) {
            switch ((UserType)value) {
                case UserType.Technician:
                    return 2;
                case UserType.Supervisor:
                    return 1;
                case UserType.Lead:
                    return 3;
                default:
                    break;
            }
            return 0;
        }
    }
}
