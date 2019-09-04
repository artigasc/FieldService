using FESA.SCM.Common.Base;

namespace FESA.SCM.WorkOrder.BE.MachineryBE
{
    public class Machinery : Entity
    {
        public string Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public decimal LifeHours { get; set; }
    }
}