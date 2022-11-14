
using VDW.SalesApp.IdentityServer.Models.Enum;

namespace VDW.SalesApp.IdentityServer.Models
{
    public class AuthenticationRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string OtpValue { get; set; }
        public Workflow WorkFlow { get; set; }

    }
}
