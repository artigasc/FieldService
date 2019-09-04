namespace FESA.SCM.ServiceGateway.DTO
{
    public class Machine
    {
        public string Id { get; set; }
        public string AssignmentId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public int TotalHoursFunction { get; set; }
        public decimal LifeHours { get; set; }
    }
}