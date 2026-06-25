using DoctorsHub.Domain.Identity;
using DoctorsHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace DoctorsHub.Web.Configurations
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddIdentityService( this IServiceCollection services) 
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options => 
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

    }
}
