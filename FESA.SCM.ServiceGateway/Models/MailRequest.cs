using FESA.SCM.ServiceGateway.DTO;

namespace FESA.SCM.ServiceGateway.Models
{
    public class MailRequest
    {
        
        public string AssignmentId { get; set; }
        public ActivityType ActivityType { get; set; }
        public string userId { get; set; }
    }

    public class MailNotification {
        public string userId { get; set; }
        public string AssignmentId { get; set; }
        public string IsClosed { get; set; }

        //
        

    }
}