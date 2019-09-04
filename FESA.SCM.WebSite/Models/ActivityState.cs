using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Models {
        public enum ActivityState {
            Active = 0,
            Completed = 1
        }

    public static class ConvertActivityState {
        public static string GetStatusSPA(object value) {
            switch ((ActivityState)value) {
                case ActivityState.Active:
                    return "Activa";
                case ActivityState.Completed:
                    return "Completada";
            }
            return string.Empty;
        }

    }

}
