namespace FESA.SCM.Core.Models
{
    public class Constants
    {
        public const string HubConnectionString =
            "Endpoint=sb://servicio-campo.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=HYPJB7ZnGmIIoxVK8unFNiArDbD4YHthfcXwkTM2i6M=";
        public const string HubPath = "fesa-notifications";

        public const string FerreyrosEncryptKey = "F354@M0b1L3";

        public const float TextboxMargin = 20f;
        public const float TextboxHeight = 60f;
        public const float ButtonHeight = 45f;
        public const float KeyboardOffset = 80f;

        public const string DatabaseName = "Assignments.db3";

        public const string TaskIdentifier = "pe.ferreyros.AssignmentSync";

        public const string NotificationsRegisterUrl = "http://api-notifications.azurewebsites.net/api/register";

        public class FesaApi
        {
            public const string BaseUrl = "http://api-gateway.azurewebsites.net/api";
            public const string LoginMethod = "users/login";
            public const string RestorePasswordMethod = "users/restorepassword";
            public const string ChangePasswordMethod = "users/changepassword";
            public const string LogOff = "users/logoff";
            public const string GetAssignments = "assignments";
            public const string SyncAssignments = "assignments/sync";
            public const string AssignmentEnded = "mails/assignmentend";
            public const string ActivityNotification = "mails/activitynotification";
            public const string Ocupability = "reports/getocupabilitylevel";
            public const string OcupabilityThroughTime = "reports/getocupabilitythroughtimeperuser";
            public const string UsersForState = "reports/getusersforstate";
            public const string OrdersPerClient = "reports/getordersperclient";
            public const string AssingmentsByClient = "reports/getassingmentsbyclient";
        }

        public class Messages
        {
            public const string DefaultError =
                "En estos momentos no podemos procesar sus solicitud, por favor intentelo de nuevo mas tarde.";

            public const string NoAccount =
                "No existe una cuenta de usuario registrada con estos datos, por favor registrese.";

            public const string PasswordError = "Las contraseñas no coinciden, por favor revisalas.";

            public const string PasswordLenghtError = "Las contraseña es muy corta, debe contener por lo menos 8 caracteres.";

            public const string ObligatoryField =
                "Los datos son obligatorios, por favor revisa que no queden datos en blanco.";

            public const string WrongAccountFormat =
                "El nombre de usuario ingresado no se encuentra en el formato adecuado, por favor revisa.";

            public const string LoginError =
                "Usuario o Contraseña incorrectos, por favor revisa.";

            public const string MailFormatError = "El formato del correo ingresado es incorrecto, por favor revisalo.";

            public const string NoNetwork =
                "No hemos podido establecer una conexión con la red del telefono, por favor revisa que estés conectado a internet para poder continuar. Gracias.";

            public const string RestoreSend =
                "En breves momentos se te envíara un correo con las instrucciones para restaurar tu contraseña";

            public const string ActivityNotFinished =
                "Tiene una actividad no culminada, por favor cierre su pendiente antes de continuar.";

            public const string CloseService =
                "¿Está seguro que desea dar por cerrado el servicio?\n Una vez cerrado no podrá modificarlo";

            public const string NotificationSent = "Se ha notificado al cliente el final del servicio.";
            public const string HasLoggedSomewhereElse = "Tiene una sesión iniciada en otro dispositivo, por favor cierrela antes de ingresar.";
        }

        public class PageKeys
        {
            public const string Login = "login";
            public const string RestorePassword = "restore_password";
            public const string ChangePassword = "change_password";
            public const string Home = "home";
            public const string Assignment = "assignment";
            public const string Documents = "documents";
            public const string ServiceData = "service_data";
            public const string ActivityTypeSelection = "activity_type_selection";
            public const string ActivityStart = "activity_start";
            public const string UsersPerState = "users_per_status";
            public const string UsersTimeLine = "user_time_line";
            public const string AssignmentsPerClient = "assignments_per_client";

        }
    }
}