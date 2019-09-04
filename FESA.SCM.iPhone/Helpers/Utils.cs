using System;
using FESA.SCM.Core.Models;

namespace FESA.SCM.iPhone.Helpers
{
    public static class Utils
    {
        public static string GetUserStatusText(Enumerations.UserStatus userStatus)
        {
            string status;

            switch (userStatus)
            {
                case Enumerations.UserStatus.Available:
                    status = "Disponible";
                    break;
                case Enumerations.UserStatus.Assigned:
                    status = "Asignado";
                    break;
                case Enumerations.UserStatus.OnWork:
                    status = "En trabajo";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(userStatus), userStatus, null);
            }
            return status;
        }

        public static string GetServiceTypeText(Enumerations.AssignmentType assignmentType)
        {
            string type;
            switch (assignmentType)
            {
                case Enumerations.AssignmentType.FieldService:
                    type = "Servicio de Campo";
                    break;
                case Enumerations.AssignmentType.Scheduled:
                    type = "Programado";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(assignmentType), assignmentType, null);
            }
            return type;
        }
    }
}