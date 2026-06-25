using DoctorsHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DoctorsHub.Web.Configurations
{
    public static class DbContextExtension
    {
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<ApplicationDbContext>(options => 
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            });

            return services;
        }

    }
}
