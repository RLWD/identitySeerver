using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VDW.SalesApp.IdentityServer.Models;

namespace VDW.SalesApp.IdentityServer.Services
{
	public class ProfileServices : IProfileService
	{
		public async Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			var claims = new List<Claim>
			{
				new Claim(UserClaimKeys.UserId, context.ValidatedRequest.Raw["UserId"]),
				new Claim(UserClaimKeys.FirstName, context.ValidatedRequest.Raw["FirstName"]),
				new Claim(UserClaimKeys.LastName, context.ValidatedRequest.Raw["LastName"]),
				new Claim(UserClaimKeys.PhoneNumber, context.ValidatedRequest.Raw["PhoneNumber"]),
				new Claim(UserClaimKeys.Email, context.ValidatedRequest.Raw["Email"]),
				new Claim(UserClaimKeys.IsActive, context.ValidatedRequest.Raw["IsActive"])
			};
			context.IssuedClaims = claims;
		}

		public async Task IsActiveAsync(IsActiveContext context)
		{
			var isActive = context.Subject.FindFirst("IsActive").Value;
			context.IsActive = isActive.ToLowerInvariant() == "true";
		}
	}
}
