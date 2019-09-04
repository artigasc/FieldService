using System;
using System.Threading.Tasks;

namespace FESA.SCM.Core.Services.Interfaces
{
    public interface INotificationService
    {
        Task<string> PostToNotificationService(string token);
        Task<bool> PutToNotificationService(string registrationId, string deviceRegistration);
    }
}