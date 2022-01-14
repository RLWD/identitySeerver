

using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Claims;
using System.Threading.Tasks;
using VDW.SalesApp.IdentityServer.Models;
using VDW.SalesApp.IdentityServer.Models.Enum;
using VDW.SalesApp.IdentityServer.Repository.Interface;

namespace VDW.SalesApp.IdentityServer.Services
{
    public class ProfileServices : IProfileService
    {
        private IUserRepository _userRepository;
        public ProfileServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            AuthenticationRequest request = GetAuthRequest(context.ValidatedRequest.Raw);
            UserClaims claim;
            if (request.WorkFlow == Workflow.OTP_LOGIN)
                claim = await _userRepository.GetUserByOtp(request.Username, request.OtpValue, 180);
            else
                claim = await _userRepository.GetUserByPassword(request.Username);

            if (claim != null)
            {
                List<Claim> claimList = new List<Claim>();
                claimList.Add(new Claim("PhoneNumber", claim.PhoneNumber));
                claimList.Add(new Claim("UserId", claim.UserId));
                claimList.Add(new Claim("RoleName", claim.Name));
                context.IssuedClaims = claimList;
            }
        }

        private AuthenticationRequest GetAuthRequest(NameValueCollection parameters)
        {
            return new AuthenticationRequest
            {
                OtpValue = parameters["otp"],
                Username = parameters["username"],
                Password = parameters["password"],
                WorkFlow = parameters["workflow"] == "OTP_LOGIN" ? Workflow.OTP_LOGIN : Workflow.PASSWORD_LOGIN
            };
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var username = context.Subject.FindFirst("sub").Value;
            var user = await _userRepository.GetPasswordByUserName(username);
            if (user != null)
            {
                context.IsActive = user.IsActive;
            }
            else
            {
                context.IsActive = false;
            }
        }
    }
}
