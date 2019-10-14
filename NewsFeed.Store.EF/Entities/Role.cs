using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsFeed.Store.EF.Entities
{
    public class Role : IdentityRole
    {
        public Role(string roleName) : base(roleName)
        {

        }
        public Role()
        {

        }

        public Role(string roleName, string description) : base(roleName)
        {
            Description = description;
        }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual ICollection<IdentityUserRole<string>> Users { get; set; }
        public virtual ICollection<IdentityRoleClaim<string>> Claims { get; set; }
    }
}
