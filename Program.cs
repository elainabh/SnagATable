using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using Snag_That_Table.Data;
using Snag_That_Table.Services.MessageServices;
using Microsoft.AspNetCore.ResponseCompression;
using Snag_That_Table.Services.RestaurantServices;
using Snag_That_Table.Services.TableServices;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddHttpClient("snag-a-table", client =>
        {
                      client.BaseAddress = new Uri("https://snag-a-table.dev/api/");
        });

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddScoped<IMessageServices, MessagesServices>();
        builder.Services.AddScoped<IRestaurantServices, RestaurantServices>();
        builder.Services.AddScoped<ITableServices, TableServices>();

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

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}