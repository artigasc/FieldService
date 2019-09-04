namespace FESA.SCM.Core.Models
{
    public class OrdersPerClient
    {
        public int Quantity { get; set; }
        public string CompanyName { get; set; }
        public Enumerations.AssignmentType AssignmentType { get; set; }
    }
}