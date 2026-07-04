using DoctorsHub.Domain.Identity;
using DoctorsHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace DoctorsHub.API.Configurations
{
    public static class AddIdentityExtension
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services) 
        {
            //Add Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options => 
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
