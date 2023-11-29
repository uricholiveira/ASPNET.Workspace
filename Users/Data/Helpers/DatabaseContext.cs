using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Helpers;

public class DatabaseContext : IdentityDbContext
{
    public const string Schema = "user";

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityUser>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("User");
        });

        modelBuilder.Entity<IdentityRole>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("Role");
        });

        modelBuilder.Entity<IdentityUserRole<string>>(builder =>
        {
            builder.HasKey(x => new { x.RoleId, x.UserId });
            builder.ToTable("UserRole");
        });

        modelBuilder.Entity<IdentityRoleClaim<string>>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("RoleClaim");
        });

        modelBuilder.Entity<IdentityUserClaim<string>>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("UserClaim");
        });

        modelBuilder.Entity<IdentityUserLogin<string>>(builder =>
        {
            builder.HasKey(x => new { x.LoginProvider, x.ProviderKey });
            builder.ToTable("UserLogin");
        });

        modelBuilder.Entity<IdentityUserToken<string>>(builder =>
        {
            builder.HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
            builder.ToTable("UserToken");
        });
    }
}