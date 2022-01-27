using IdentityModel;
using IdentityServer4.Validation;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VDW.SalesApp.IdentityServer.Models.Enum;

namespace VDW.SalesApp.IdentityServer.Services
{
    public class ResourceOwnerServices : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            List<Claim> claimList = new List<Claim>();
            claimList.Add(new Claim("PhoneNumber", context.Request.Raw["PhoneNumber"]));
            claimList.Add(new Claim("UserId", context.Request.Raw["UserId"]));
            claimList.Add(new Claim("RoleName", context.Request.Raw["RoleName"]));
            claimList.Add(new Claim("IsActive", context.Request.Raw["IsActive"]));

            if (context.Request.Raw["Otp"] == Workflow.OTP_LOGIN.ToString())
            {
  
                context.Result = new GrantValidationResult(context.Request.Raw["PhoneNumber"], OidcConstants.GrantTypes.Password, claimList, "local", new Dictionary<string, object>() { { "ForceChangeRequired", true } });
            }
            else
            {
                context.Result = new GrantValidationResult(context.Request.Raw["PhoneNumber"], OidcConstants.GrantTypes.Password, claimList, "local", new Dictionary<string, object>() { { "ForceChangeRequired", false } });
            }

        }
    }
}
