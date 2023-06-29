using IdentityAppCourse2022.Data;
using IdentityAppCourse2022.helpers;
using IdentityAppCourse2022.interfaces;
using IdentityAppCourse2022.Models;
using IdentityAppCourse2022.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DbConnectionString");
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(connectionString);
});
 
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ISendGrid_Email, SendGrid_Email>();
builder.Services.Configure<AuthMessageSendOptions>(builder.Configuration.GetSection("SendGrid"));
builder.Services.Configure<IdentityOptions>(opt => 
{
    opt.Password.RequiredLength = 5;
    opt.Password.RequireLowercase=true;
    opt.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromSeconds(10);
    opt.Lockout.MaxFailedAccessAttempts=5;
    //opt.SignIn.RequireConfirmedAccount=true;

});

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

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
