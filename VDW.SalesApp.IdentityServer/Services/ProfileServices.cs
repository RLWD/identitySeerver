

using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VDW.SalesApp.IdentityServer.Services
{
    public class ProfileServices : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            List<Claim> claimList = new List<Claim>();
            claimList.Add(new Claim("PhoneNumber", context.ValidatedRequest.Raw["PhoneNumber"]));
            claimList.Add(new Claim("UserId", context.ValidatedRequest.Raw["UserId"]));
            claimList.Add(new Claim("RoleName", context.ValidatedRequest.Raw["RoleName"]));
            claimList.Add(new Claim("RoleName", context.ValidatedRequest.Raw["IsActive"]));
            claimList.Add(new Claim("PermissionList", context.ValidatedRequest.Raw["rolePermissions"]));
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var isActive = context.Subject.FindFirst("IsActive").Value;
            context.IsActive = isActive.ToLowerInvariant()=="true";
        }
    }
}
