using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Models
{
    public class UserModel
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
            public RoleModel Role { get; set; }
            public UserType UserType { get; set; }

            public int UserStatus { get; set; }


            public int OfficeId { get; set; }
            public int CostCenterId { get; set; }
            public string Office { get; set; }
            public string CostCenter { get; set; }
            public string Celullar { get; set; }
            public string Rpm { get; set; }

            public string Token { get; set; }
    }
}
