using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_Test.Data
{
    public class User:IdentityUser
    {
        public User() { 
            
        }
        public string TestString { get; set; }
        public virtual ICollection<UserRole> Roles { get; } = new List<UserRole>();
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; } = new List<IdentityUserClaim<string>>();
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; } = new List<IdentityUserLogin<string>>();
    }

    public class Role : IdentityRole { 
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
    public class UserRole : IdentityUserRole<string> { 
        public Role Role { get; set; }
        public User User { get; set; }
    }

}
