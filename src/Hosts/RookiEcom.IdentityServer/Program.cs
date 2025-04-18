using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RookiEcom.IdentityServer.ConfigurationOptions;
using RookiEcom.IdentityServer.Controllers;
using RookiEcom.IdentityServer.Domain;
using RookiEcom.IdentityServer.Extensions;
using RookiEcom.IdentityServer.Persistence;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var appSettings = new AppSettings();
configuration.Bind(appSettings);

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(appSettings.ConnectionStrings.Identity,
        sql => sql.MigrationsAssembly(typeof(Program).Assembly.FullName)));

builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
    })
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddIdentityServer(options =>
        options.KeyManagement.Enabled = false
    )
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(appSettings.ConnectionStrings.Identity,
                sql => sql.MigrationsAssembly(typeof(Program).Assembly.FullName));
    }) 
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(appSettings.ConnectionStrings.Identity,
                sql => sql.MigrationsAssembly(typeof(Program).Assembly.FullName));
        options.EnableTokenCleanup = true;
    })
    .AddAspNetIdentity<User>()
    .AddProfileService<CustomProfileService>()
    .AddDeveloperSigningCredential();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";

        options.Events.OnValidatePrincipal = async context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<AccountController>>();
            var requestCookies = context.HttpContext.Request.Cookies;
            logger.LogInformation("Request cookies during OnValidatePrincipal: {Cookies}",
                string.Join(", ", requestCookies.Select(c => $"{c.Key}: {c.Value}")));

            if (context.Principal != null)
            {
                logger.LogInformation("OnValidatePrincipal: User authenticated with claims: {Claims}",
                    string.Join(", ", context.Principal.Claims.Select(c => $"{c.Type}: {c.Value}")));
            }
            else
            {
                logger.LogWarning("OnValidatePrincipal: No authenticated user found.");
            }
            await Task.CompletedTask;
        };
        
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClients", policy =>
        policy.WithOrigins("https://localhost:5001", "https://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowClients");

app.UseRouting();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();