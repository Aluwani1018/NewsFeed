using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsFeed.Store.EF.Entities;

namespace NewsFeed.Store.EF
{
    public class NewsFeedDbContext : IdentityDbContext<User, Role, string>
    {
        public NewsFeedDbContext(DbContextOptions<NewsFeedDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().HasMany(u => u.Claims).WithOne().HasForeignKey(c => c.UserId).IsRequired();
            builder.Entity<User>().HasMany(u => u.Roles).WithOne().HasForeignKey(r => r.UserId).IsRequired();

            builder.Entity<User>().ToTable("User");

            builder.Entity<Role>().ToTable("Role");

            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");

            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");


        }
    }
}
