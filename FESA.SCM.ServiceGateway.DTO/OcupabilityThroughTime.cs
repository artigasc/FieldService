using System;

namespace FESA.SCM.ServiceGateway.DTO
{
    public class OcupabilityThroughTime
    {
        public int Quantity { get; set; }
        public string UserId { get; set; }
        public DateTime AssignmentDate { get; set; }
        public string ShowedDate { get; set; }
        public UserStatus status { get; set; }
    }
}