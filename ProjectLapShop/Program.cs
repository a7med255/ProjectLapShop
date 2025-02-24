using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ProjectLapShop.Bl;
using ProjectLapShop.Models;
using ProjectLapShop.Utilities;
using Stripe;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();//resonsiple carbage
builder.Services.AddDbContext<LapShopContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));//to get connect sql in app.json
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
    options =>
    {
        //options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.User.RequireUniqueEmail = true;
    }).AddEntityFrameworkStores<LapShopContext>().AddDefaultTokenProviders();//System Identity
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    options.TokenLifespan = TimeSpan.FromMinutes(3)); // Token valid for 3 minutes

builder.Services.AddScoped<ICategories,ClsCategories>();
builder.Services.AddScoped<IItems, ClsItems>();
builder.Services.AddScoped<IItemTypes, ClsItemTypes>();
builder.Services.AddScoped<IOs, ClsOs>();
builder.Services.AddScoped<IItemImages, ClsItemImages>();
builder.Services.AddScoped<ISliders, ClsSliders>();
builder.Services.AddScoped<ISalesInvoice, ClsSalesInvoice>();
builder.Services.AddScoped<ISalesInvoiceItems, ClsSalesInvoiceItems>();
builder.Services.AddScoped<IPages, ClsPages>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IWishlistService, WishlistService>();




builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();//request & response
builder.Services.AddDistributedMemoryCache();//if any problem cached in server

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Users/AccessDenied";
    options.Cookie.Name = "Cookie";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
    options.LoginPath = "/Users/Login";
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});//for returnUrl

var app = builder.Build();
    
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
app.UseSession();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<String>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}");

    endpoints.MapControllerRoute(
    name: "LandingPages",
    pattern: "{area:exists}/{controller=Home}/{action=Index}");

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
    name: "ali",
    pattern: "ali/{controller=Home}/{action=Index}/{id?}");



}
);

app.Run();
