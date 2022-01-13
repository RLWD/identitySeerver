using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using VDW.SalesApp.IdentityServer.Models;
using VDW.SalesApp.IdentityServer.Models.Enum;
using VDW.SalesApp.IdentityServer.Provider;
using VDW.SalesApp.IdentityServer.Repository.Interface;

namespace VDW.SalesApp.IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        protected readonly IUserRepository _userRepository;

        public ProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            AuthenticationRequest request = GetAuthRequest(context.ValidatedRequest.Raw);
            UserClaims claim;

            if (request.WorkFlow == Workflow.OTP)
            {
                claim = await _userRepository.GetUserByOtp(request.Username, request.OtpValue);
            }
            else
            {
                User user = await _userRepository.GetPasswordByUserName(request.Username);
                if (PasswordProvider.Verify(user, request.Password))
                {
                    claim = await _userRepository.GetUserByPassword(request.Username);
                }
                else
                {
                    throw new AuthenticationException("Unable To Validate Credential");
                }
            }
            var claims = new List<Claim>();

            if (claim != null)
            {
                claims.Add(new Claim("PhoneNumber", claim.PhoneNumber));
                claims.Add(new Claim("UserId", claim.UserId));
                claims.Add(new Claim("RoleName", claim.Name));
            }
            context.IssuedClaims = claims;
        }

        private AuthenticationRequest GetAuthRequest(NameValueCollection parameters)
        {
            return new AuthenticationRequest
            {
                OtpValue = parameters["otp"],
                Username = parameters["username"],
                Password = parameters["password"],
                WorkFlow = parameters["workflow"] == "OTP" ? Workflow.OTP : Workflow.PASSWORD
            };
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            string username = context.Subject.FindFirstValue("sub");
            var user = await _userRepository.GetPasswordByUserName(username);
            context.IsActive = user.IsActive;
        }
    }
}
