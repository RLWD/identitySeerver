using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores.Serialization;
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
				new Claim(UserClaimKeys.EnglishName, context.ValidatedRequest.Raw["EnglishName"]),
				new Claim(UserClaimKeys.ChineseName, context.ValidatedRequest.Raw["ChineseName"]),
				new Claim(UserClaimKeys.PhoneNumber, context.ValidatedRequest.Raw["PhoneNumber"]),
				new Claim(UserClaimKeys.Email, context.ValidatedRequest.Raw["Email"]),
				new Claim(UserClaimKeys.IsActive, context.ValidatedRequest.Raw["IsActive"]),
				new Claim(UserClaimKeys.UserCode, context.ValidatedRequest.Raw["UserCode"])
			};
            if (!string.IsNullOrEmpty(context.ValidatedRequest.Raw["CustomerId"]))
                claims.Add(new Claim(UserClaimKeys.CustomerId, context.ValidatedRequest.Raw["CustomerId"]));
            if (!string.IsNullOrEmpty(context.ValidatedRequest.Raw["WechatUserId"]))
                claims.Add(new Claim(UserClaimKeys.WechatUserId, context.ValidatedRequest.Raw["WechatUserId"]));
            if (!string.IsNullOrEmpty(context.ValidatedRequest.Raw["Wechat"]))
                claims.Add(new Claim(UserClaimKeys.WechatPermission, context.ValidatedRequest.Raw["Wechat"]));
            context.IssuedClaims = claims;
		}

		public async Task IsActiveAsync(IsActiveContext context)
		{
			var isActive = context.Subject.FindFirst("IsActive").Value;
			context.IsActive = isActive.ToLowerInvariant() == "true";
		}
	}
}
