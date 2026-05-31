using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.StaticFiles;
using Journal.Data;
using Ganss.Xss;
using Microsoft.AspNetCore.Identity;
using System.Net.Security;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Journal.Data.JournalContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("JournalContext") ?? throw new InvalidOperationException("Connection string 'JournalContext' not found.")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 16;
    options.Password.RequiredUniqueChars = 1;
    
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
})

.AddEntityFrameworkStores<JournalContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IHtmlSanitizer, HtmlSanitizer>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/access-denied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.WebHost.ConfigureKestrel(serverConfig =>
{
    serverConfig.AddServerHeader = false;
}
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/error");

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();
app.UseStaticFiles();

app.MapRazorPages(); 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    
app.Run();