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

        // Add services to the container.
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

        //builder.Services.AddScoped<IBookService, BookService>();

        builder.Services.AddDbContext<ReadingJournalDbContext>(options =>
        {
            options.UseSqlServer(
               "Server=DESKTOP-F3IKLD2;Database=ReadingJournalDb;Trusted_Connection=True;");
            //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
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
            // Cookie settings
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            // We should fix the URLs by adding Scafolded Identity or our own html files!
            options.LoginPath = "/user/login";
            options.LogoutPath = "/user/logout";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.SlidingExpiration = true;
        });

        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 5;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;

            // Log in required.
            options.SignIn.RequireConfirmedAccount = false; // default
            options.SignIn.RequireConfirmedEmail = false; // default
        });


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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