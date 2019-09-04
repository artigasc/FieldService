using FESA.SCM.Common.Base;
using FESA.SCM.Identity.BE.RoleBE;


namespace FESA.SCM.Identity.BE.UserBE
{
    public class User : Entity
    {
        public string Id { get; set; }
        public string FesaUserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Pns { get; set; }
        public string Password { get; set; }
        public string Photo { get; set; }
        public bool ChangedPassword { get; set; }
        public bool SessionActive { get; set; }
        public Role Role { get; set; }
        public UserType UserType { get; set; }

        public int UserStatus { get; set; }


        public int OfficeId { get; set; }
        public int CostCenterId { get; set; }
        public string Office { get; set; }
        public string CostCenter { get; set; }
        public string Celullar { get; set; }
        public string Rpm { get; set; }

    }
}