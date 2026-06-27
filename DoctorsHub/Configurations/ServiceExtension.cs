using DoctorHub.Application.Interfaces;
using DoctorHub.Application.Services;
using DoctorsHub.Application.Interfaces;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Services;
using DoctorsHub.Infrastructure.Repositories;

namespace DoctorsHub.Web.Configurations
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
            //Adding Bussiness Services to IOC Container
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<ISpecializationService, SpecializationService>();

            //Adding Repository Service to IOC container
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();


            return services;
        }
    }
}
