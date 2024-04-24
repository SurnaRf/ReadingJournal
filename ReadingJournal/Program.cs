using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReadingJournal.Pages;
using ReadingJournal.Services;
using ServiceLayer;
using MudBlazor.Services;

namespace ReadingJournal;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        string connectionString = builder.Configuration["ConnectionString"];

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        builder.Services.AddSingleton<SeederManager>();
        builder.Services.AddSingleton<SeederService>();
        builder.Services.AddAuthenticationCore();

        builder.Services.AddScoped<GenreManager, GenreManager>();
        builder.Services.AddScoped<GenreContext, GenreContext>();
		builder.Services.AddScoped<PublisherManager, PublisherManager>();
        builder.Services.AddScoped<PublisherContext, PublisherContext>();
        builder.Services.AddScoped<AuthorContext, AuthorContext>();
        builder.Services.AddScoped<AuthorManager, AuthorManager>();
        builder.Services.AddScoped<ShelfManager, ShelfManager>();
        builder.Services.AddScoped<ShelfContext, ShelfContext>();
        builder.Services.AddScoped<BookManager, BookManager>();
        builder.Services.AddScoped<BookContext, BookContext>();
        builder.Services.AddScoped<UserManager, UserManager>();
        builder.Services.AddScoped<UserContext, UserContext>();       
        builder.Services.AddScoped<EditionManager, EditionManager>();
        builder.Services.AddScoped<EditionContext, EditionContext>();
        builder.Services.AddScoped<FriendRequestManager, FriendRequestManager>();
        builder.Services.AddScoped<FriendRequestContext, FriendRequestContext>();

        builder.Services.AddScoped<ErrorModel, ErrorModel>();
        builder.Services.AddScoped<BookService, BookService>();

        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        builder.Services.AddScoped<ProtectedSessionStorage, ProtectedSessionStorage>();

        builder.Services.AddMudServices();

        builder.Services.AddDbContext<ReadingJournalDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
         });
               
        builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ReadingJournalDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

        builder.Services.AddScoped(sp =>
            new HttpClient
            {
                BaseAddress = new Uri(builder.Configuration["BookAPI"])
            });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            options.LoginPath = "/user/login";
            options.LogoutPath = "/user/logout";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.SlidingExpiration = true;
        });

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 5;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.AllowedForNewUsers = true;

            options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;

            options.SignIn.RequireConfirmedAccount = false; 
            options.SignIn.RequireConfirmedEmail = false; 
        });


        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}