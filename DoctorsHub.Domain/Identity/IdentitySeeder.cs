using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Domain.Identity
{
    public static class IdentitySeeder
    {
        public async static Task SeedRolesAndAdminAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) 
        {
            string[] roles = { "Patient", "Doctor", "Admin"};

            foreach (var role in roles) 
            {
                if (! await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            //create Admin User
            var adminEmail = "admin@DoctorsHub.com";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null) 
            {
                adminUser = new ApplicationUser()
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "Admin123");
                if (result.Succeeded) 
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
