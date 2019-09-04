using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FESA.SCM.Web.Models
{
    [Table("USER")]
    public class User
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public Enumerations.UserType UserType { get; set; }
        public Enumerations.UserStatus UserStatus { get; set; }
    
    }
}