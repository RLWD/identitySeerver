using IdentityModel;
using IdentityServer4.Validation;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VDW.SalesApp.IdentityServer.Models;

namespace VDW.SalesApp.IdentityServer.Services
{
	public class ResourceOwnerServices : IResourceOwnerPasswordValidator
	{
		public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
		{
			var claimList = new List<Claim>()
			{
				new Claim(UserClaimKeys.UserId, context.Request.Raw["UserId"]),
				new Claim(UserClaimKeys.EnglishName, context.Request.Raw["EnglishName"]),
				new Claim(UserClaimKeys.ChineseName, context.Request.Raw["ChineseName"]),
				new Claim(UserClaimKeys.PhoneNumber, context.Request.Raw["PhoneNumber"]),
				new Claim(UserClaimKeys.Email, context.Request.Raw["Email"]),
				new Claim(UserClaimKeys.IsActive, context.Request.Raw["IsActive"]),
				new Claim(UserClaimKeys.UserCode, context.Request.Raw["UserCode"]),
            };
			if (!string.IsNullOrEmpty(context.Request.Raw["CustomerId"]))
				claimList.Add(new Claim(UserClaimKeys.CustomerId, context.Request.Raw["CustomerId"]));
            if (!string.IsNullOrEmpty(context.Request.Raw["Wechat"]))
                claimList.Add(new Claim(UserClaimKeys.WechatPermission, context.Request.Raw["Wechat"]));

            context.Result = new GrantValidationResult(
				subject: context.Request.Raw["PhoneNumber"],
				authenticationMethod: OidcConstants.GrantTypes.Password,
				claims: claimList);
			
		}
	}
}