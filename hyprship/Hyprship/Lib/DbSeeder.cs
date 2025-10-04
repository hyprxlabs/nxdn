using Hyprship.Data.Models;

using Hyprx.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hyprship;

public class DbSeeder
{
    private readonly IPasswordHasher<User> hasher;

    public DbSeeder(IPasswordHasher<User> passwordHasher)
    {
        this.hasher = passwordHasher;
    }

    public async Task SeedAsync(DbContext db, CancellationToken cancellationToken = default)
    {
        var roleNames = new List<string>()
        {
            "admin",
            "reader",
            "auditor",
        };

        Role adminRole = new Role();
        var changed = false;
        foreach (var roleName in roleNames)
        {
            var upcase = roleName.ToUpper();
            var hasRole = await db.Set<Role>().AnyAsync(o => o.UpcaseName == upcase, cancellationToken);
            if (!hasRole)
            {
                var role = new Role(roleName);
                db.Set<Role>().Add(role);
                if (roleName == "admin")
                    adminRole = role;
            }

            changed = true;
        }

        if (changed)
        {
            changed = false;
            await db.SaveChangesAsync(cancellationToken);
        }

        var email = Environment.GetEnvironmentVariable("HS_ADMIN_EMAIL") ?? "admin@localhost";
        var username = Environment.GetEnvironmentVariable("HS_ADMIN_USER") ?? "admin";
        var password = Environment.GetEnvironmentVariable("HS_ADMIN_PASSWORD") ?? "changeM3!";
        var upcaseEmail = email.ToUpperInvariant();

        if (!await db.Set<User>().AnyAsync(o => o.UpcaseEmail == upcaseEmail, cancellationToken))
        {
            var user = new User(email, username);
            user.CreatedAt = DateTime.UtcNow;
            user.IsServiceAccount = true;
            var pd = this.hasher.HashPassword(user, password);
            var auth = new UserPasswordAuth(user.Id, pd);
            user.PasswordAuth = auth;
            auth.IsEmailVerified = true;
            auth.IsTwoFactorEnabled = false;
            auth.CreatedAt = DateTime.UtcNow;
            user.Roles.Add(adminRole);

            db.Set<User>().Add(user);
            db.Set<UserPasswordAuth>().Add(auth);
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}