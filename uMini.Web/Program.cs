var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews(config =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    config.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AbsoluteShortUrlResolver>();
builder.Services.AddToastrNotifications(options =>
{
    options.PositionClass = "toast-bottom-right";
});

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddAutoMapper(typeof(MappingProfiles));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHostUrlMiddleware();

app.MapAreaControllerRoute(
    name: "Identity",
    areaName: "Identity",
    pattern: "i/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "User",
    areaName: "User",
    pattern: "u/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "a/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Logger.LogInformation("Seeding Database...");

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var shortUrlContext = scopedProvider.GetRequiredService<ShortUrlDbContext>();

        await shortUrlContext.Database.MigrateAsync();
        await ShortUrlDbContextSeed.SeedAsync(shortUrlContext, app.Logger);

        var identityContext = scopedProvider.GetRequiredService<ApplicationIdentityDbContext>();
        var userManager = scopedProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await ApplicationIdentityDbContextSeed.SeedAsync(identityContext, userManager, roleManager);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Logger.LogInformation("Launching");
app.Run();
