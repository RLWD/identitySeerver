
using System.Threading.Tasks;
using VDW.SalesApp.IdentityServer.Models;

namespace VDW.SalesApp.IdentityServer.Repository.Interface
{
    public interface IUserRepository
    {
        public Task<UserClaims> GetUserByOtp(string phoneNumber, string tokenValue, int tokenExpirySeconds);
        public Task<UserClaims> GetUserByPassword(string phoneNumber);
        public Task<User> GetPasswordByUserName(string userName);
        void LogFailedAttempt(string userName, string failReason);
    }
}
