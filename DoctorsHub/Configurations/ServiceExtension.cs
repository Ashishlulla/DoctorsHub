using DoctorHub.Application.Interfaces;
using DoctorHub.Application.Services;
using DoctorsHub.Application.Interfaces;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Mapping;
using DoctorsHub.Application.Services;
using DoctorsHub.Infrastructure.Repositories;
using AutoMapper;

namespace DoctorsHub.Web.Configurations
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
            //Injecting Bussiness Services to IOC Container
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IPatientService, PatientService>();

            services.AddScoped<ISpecializationService, SpecializationService>();

            //Injecting Repository Service to IOC container
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();


            return services;
        }
    }
}
