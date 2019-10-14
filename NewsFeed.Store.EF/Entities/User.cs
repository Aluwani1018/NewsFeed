using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace NewsFeed.Store.EF.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsLockedOut => this.LockoutEnabled && this.LockoutEnd >= DateTimeOffset.UtcNow;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
