

namespace VDW.SalesApp.IdentityServer.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
    }
}
