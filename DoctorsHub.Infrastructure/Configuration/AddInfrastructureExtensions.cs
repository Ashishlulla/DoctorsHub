using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Domain.Identity;
using DoctorsHub.Infrastructure.Migrations;
using DoctorsHub.Infrastructure.Persistence;
using DoctorsHub.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DoctorsHub.Infrastructure.Configurations 
{
    public static class AddInfrastructureExtensions 
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
        {
            //Add DbContext
            services.AddDbContext<ApplicationDbContext>(options => 
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            });


            //Add Repository Services
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IPatientRepository,PatientRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();
            services.AddScoped<ICRMRepository, CRMRepository>();
            return services;
        }
    }
}