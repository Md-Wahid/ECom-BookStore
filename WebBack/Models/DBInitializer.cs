using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WebBack.Areas.Identity.Data;

namespace WebBack.Models
{
    public static class DBInitializer
    {
        public static async Task InitializeAsync(DataContext context, IServiceProvider serviceProvider, UserManager<AppUser> userManager)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] RoleNames = { "Admin", "User" };
            IdentityResult roleResult;
            foreach (var roleName in RoleNames)
            {
                var roleExists = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            string email = "admin@email.com";
            string password = "Admin-1234";
            if (userManager.FindByEmailAsync(email).Result == null)
            {
                AppUser user = new AppUser();
                user.UserName = email;
                user.Email = email;
                IdentityResult result = userManager.CreateAsync(user, password).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}