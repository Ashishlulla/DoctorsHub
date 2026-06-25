using DoctorHub.Application.Interfaces;
using DoctorHub.Application.Services;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Infrastructure.Repositories;

namespace DoctorsHub.Web.Configurations
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
            //Adding Bussiness Services to IOC Container
            services.AddScoped<IDoctorService, DoctorService>();

            //Adding Repository Service to IOC container
            services.AddScoped<IDoctorRepository, DoctorRepository>();

            return services;
        }
    }
}
