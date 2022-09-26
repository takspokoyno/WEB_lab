using Labka1;
using Labka1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RacingContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("Default Connection")));

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<IdentityContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();

builder.Services.AddAuthentication()
        .AddGoogle(opts =>
        {
            opts.ClientId = "642336707671-73jnbecnuuj8erf9vuvns3rapbt3hgtn.apps.googleusercontent.com";
            opts.ClientSecret = "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";
            opts.SignInScheme = IdentityConstants.ExternalScheme;
        });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await RoleInitializer.InitializeAsync(userManager, rolesManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database." + DateTime.Now.ToString());
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication(); //підключення автентифікації

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();