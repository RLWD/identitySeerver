using IdentityModel;
using IdentityServer4.Validation;
using System.Threading.Tasks;
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
            var claim = await _userRepository.GetUserByPassword(context.UserName);
            context.Result = new GrantValidationResult(claim.PhoneNumber, OidcConstants.GrantTypes.Password);
        }
    }
}
