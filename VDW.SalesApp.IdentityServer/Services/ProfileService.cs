using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Threading.Tasks;

namespace VDW.SalesApp.IdentityServer.Services
{
	public class ProfileService : IProfileService
	{
		public Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			// Call get user api & assign to claims

			throw new NotImplementedException();
		}

		public Task IsActiveAsync(IsActiveContext context)
		{
			// Get user by user id & validate user !=null & isactive

			throw new NotImplementedException();
		}
	}
}
