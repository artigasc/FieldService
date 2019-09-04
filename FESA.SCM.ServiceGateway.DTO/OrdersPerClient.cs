namespace FESA.SCM.ServiceGateway.DTO
{
    public class OrdersPerClient
    {
        public string OrderId { get; set; }
        public int Quantity { get; set; }
        public string CompanyName { get; set; }
        public AssignmentType AssignmentType { get; set; }
    }
}