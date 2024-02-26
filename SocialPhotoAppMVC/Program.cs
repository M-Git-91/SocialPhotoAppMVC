global using SocialPhotoAppMVC.Data;
global using SocialPhotoAppMVC.Models;
global using SocialPhotoAppMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SocialPhotoAppMVC.Helpers;
using SocialPhotoAppMVC.Services;
using SocialPhotoAppMVC.Services.AlbumService;
using SocialPhotoAppMVC.Services.CommentService;
using SocialPhotoAppMVC.Services.PhotoService;
using SocialPhotoAppMVC.Services.SearchService;
using SocialPhotoAppMVC.Services.UserService;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging();
});
    
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Services/Repositories
builder.Services.AddScoped<IPhotoService, PhotoService >();
builder.Services.AddScoped<ICloudService, CloudService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICommentService, CommentService>();

//Identity
builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = "1078928018442-ib105qbqfl00q7807skqahm7hlofueva.apps.googleusercontent.com";
        googleOptions.ClientSecret = "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";
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

//Cloudinary
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
