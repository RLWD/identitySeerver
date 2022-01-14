using IdentityModel;
using IdentityServer4.Validation;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Claims;
using System.Threading.Tasks;
using VDW.SalesApp.IdentityServer.Models;
using VDW.SalesApp.IdentityServer.Models.Enum;
using VDW.SalesApp.IdentityServer.Provider;
using VDW.SalesApp.IdentityServer.Repository.Interface;

namespace VDW.SalesApp.IdentityServer.Services
{
    public class ResourceOwnerServices : IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository _userRepository;
        public ResourceOwnerServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            AuthenticationRequest request = GetAuthRequest(context.Request.Raw);
            UserClaims claim;
            if (request.WorkFlow == Workflow.OTP_LOGIN)
            {
                claim = await _userRepository.GetUserByOtp(request.Username, request.OtpValue, 180);
                if (claim == null)
                {
                    _userRepository.LogFailedAttempt(request.Username, "Expired OTP");
                    context.Result = new GrantValidationResult { IsError = true, Error = "INVALID_OTP", ErrorDescription = "Expired OTP" };
                }
                else
                {
                    List<Claim> claimList = new List<Claim>();
                    claimList.Add(new Claim("PhoneNumber", claim.PhoneNumber));
                    claimList.Add(new Claim("UserId", claim.UserId));
                    claimList.Add(new Claim("RoleName", claim.Name));
                    context.Result = new GrantValidationResult(claim.PhoneNumber, OidcConstants.GrantTypes.Password, claimList, "local", new Dictionary<string, object>() { { "ForceChangeRequired", true } });
                }
            }
            else
            {
                User user = await _userRepository.GetPasswordByUserName(context.UserName);

                if (user == null || user.UserName != context.UserName)
                {
                    _userRepository.LogFailedAttempt(context.UserName, "User Does not exist");
                    context.Result = new GrantValidationResult { IsError = true, Error = "NO_USER", ErrorDescription = "User Does not exist" };
                }
                else
                {
                    if (PasswordProvider.Verify(user, context.Password))
                    {
                        if (!user.IsActive)
                        {
                            _userRepository.LogFailedAttempt(context.UserName, "Deactivated User or User is not Activated");
                            context.Result = new GrantValidationResult { IsError = true, Error = "NOT_ACTIVE", ErrorDescription = "Deactivated User or User is not Activated" };
                        }
                        else
                        {
                            claim = await _userRepository.GetUserByPassword(context.UserName);
                            List<Claim> claimList = new List<Claim>();
                            claimList.Add(new Claim("PhoneNumber", claim.PhoneNumber));
                            claimList.Add(new Claim("UserId", claim.UserId));
                            claimList.Add(new Claim("RoleName", claim.Name));

                            context.Result = new GrantValidationResult(claim.PhoneNumber, OidcConstants.GrantTypes.Password, claimList, "local", new Dictionary<string, object>() { { "ForceChangeRequired", false } });
                        }
                    }
                    else
                    {
                        _userRepository.LogFailedAttempt(context.UserName, "Wrong Password");
                        context.Result = new GrantValidationResult { IsError = true, Error = "INVALID_CREDENTIAL", ErrorDescription = "Unable to Verify the credential" };
                    }
                }
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
    }
}
