using Microsoft.AspNetCore.Identity;
using VDW.SalesApp.IdentityServer.Models;

namespace VDW.SalesApp.IdentityServer.Provider
{
    internal class PasswordProvider
    {
        internal static string Encrypt(User user, string password)
        {
            var hasher = new PasswordHasher<User>();
            var hashedPassword = hasher.HashPassword(user, password);
            return hashedPassword;
        }

        internal static bool Verify(User user, string password)
        {
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.Password, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
