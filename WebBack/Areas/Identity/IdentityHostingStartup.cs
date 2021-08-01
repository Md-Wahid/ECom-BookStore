using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebBack.Areas.Identity.Data;

[assembly: HostingStartup(typeof(WebBack.Areas.Identity.IdentityHostingStartup))]
namespace WebBack.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<DataContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("DataContextConnection")));

                services.AddDefaultIdentity<AppUser>().AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<DataContext>();
            });
        }
    }
}