namespace uMini.Infrastructure.Identity;

public class ApplicationIdentityDbContextSeed
{
    public static async Task SeedAsync(ApplicationIdentityDbContext context, UserManager<IdentityUser> userManager)
    {
        var defaultUser = new IdentityUser
        {
            Id = "472963c0-6192-4f48-9006-11761bf99657",
            UserName = "demouser@microsoft.com",
            Email = "demouser@microsoft.com",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
        };

        if (!context.Users.Any(u => u.Email == defaultUser.Email))
        {
            var password = new PasswordHasher<IdentityUser>();
            var hashed = password.HashPassword(defaultUser, "Passw0rd!");
            defaultUser.PasswordHash = hashed;

            await userManager.CreateAsync(defaultUser);
        }
    }

}
