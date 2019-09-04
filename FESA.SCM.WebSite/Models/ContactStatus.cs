namespace FESA.SCM.WebSite.Models {
    public enum ContactStatus {
        Assigned = 1,
        InProcess = 2,
        Completed = 3,
        StandBy = 4,
        
    }

    public static class ConvertContactStatus {
        public static string GetStatusSPA(object value) {
            switch ((ContactStatus)value) {
                case ContactStatus.Assigned:
                    return "Asignado";
                case ContactStatus.InProcess:
                    return "En Proceso";
                case ContactStatus.Completed:
                    return "Completado";
                case ContactStatus.StandBy:
                    return "Stand-By";
            }
            return string.Empty;
        }

    }
}