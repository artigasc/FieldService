using FESA.SCM.WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> Login(string username, string password);
    }
}
