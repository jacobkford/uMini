namespace uMini.Infrastructure.Identity;

public class ApplicationIdentityDbContextSeed
{
    public static async Task SeedAsync(ApplicationIdentityDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (!context.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole(ApplicationUserRole.SuperAdmin));
            await roleManager.CreateAsync(new IdentityRole(ApplicationUserRole.Admin));
            await roleManager.CreateAsync(new IdentityRole(ApplicationUserRole.Moderator));
            await roleManager.CreateAsync(new IdentityRole(ApplicationUserRole.Member));
        }

        var defaultUser = new ApplicationUser
        {
            Id = "472963c0-6192-4f48-9006-11761bf99657",
            UserName = "admin",
            Email = "demouser@microsoft.com",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
        };

        if (!context.Users.Any(u => u.Email == defaultUser.Email))
        {
            /*var password = new PasswordHasher<ApplicationUser>();
            var hashed = password.HashPassword(defaultUser, "Passw0rd!");
            defaultUser.PasswordHash = hashed;*/

            await userManager.CreateAsync(defaultUser, "Passw0rd!");
            await userManager.AddToRoleAsync(defaultUser, ApplicationUserRole.SuperAdmin);
            await userManager.AddToRoleAsync(defaultUser, ApplicationUserRole.Admin);
            await userManager.AddToRoleAsync(defaultUser, ApplicationUserRole.Moderator);
            await userManager.AddToRoleAsync(defaultUser, ApplicationUserRole.Member);
        }
    }

}
