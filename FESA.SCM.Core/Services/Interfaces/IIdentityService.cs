using System.Threading;
using System.Threading.Tasks;
using FESA.SCM.Core.Models;

namespace FESA.SCM.Core.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<User> LoginAsync(string userName, string password,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> ResetPasswordAsync(string userName, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> ChangePasswordAsync(string userId, string newPassword, CancellationToken cancellationToken = default(CancellationToken));

        Task<User> GetUserDataAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task UpdateUserDataAsync(Enumerations.UserStatus status, CancellationToken cancellationToken = default(CancellationToken));
        Task<byte[]> DownloadUserImage(string imageUrl);
        Task LogOffAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}