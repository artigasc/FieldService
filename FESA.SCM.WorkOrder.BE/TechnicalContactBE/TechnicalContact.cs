using FESA.SCM.Common.Base;

namespace FESA.SCM.WorkOrder.BE.TechnicalContactBE
{
    public class TechnicalContact : Entity
    {
        public string Id { get; set; }
        public string ContactId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Charge { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string CustomerId { get; set; }

    }
}