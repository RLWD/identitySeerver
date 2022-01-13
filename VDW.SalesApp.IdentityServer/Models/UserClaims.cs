using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VDW.SalesApp.IdentityServer.Models
{
    public class UserClaims
    {
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
    }
}
