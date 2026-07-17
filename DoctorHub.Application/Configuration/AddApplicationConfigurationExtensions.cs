using DoctorHub.Application.DTOs.Doctors;
using DoctorHub.Application.Services;
using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.DTOs.Authentication;
using DoctorsHub.Application.DTOs.Patients;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Application.Mapping;
using DoctorsHub.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;


namespace DoctorsHub.Application.Configuration
{
    public static class AddApplicationConfigurationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) 
        {

            //Adding Service Extensions
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<ISpecializationService, SpecializationService>();
            services.AddScoped<ICRMService, CRMService>();
            services.AddScoped<IBusinessInsightsService, BusinessInsightsService>();
            services.AddScoped<IAuthService, AuthService>();


            //Adding Validators services

            //Doctor Validator
            services.AddValidatorsFromAssemblyContaining<CreateDoctorDto>();
            services.AddValidatorsFromAssemblyContaining<UpdateDoctorDto>();

            //Patient Validator
            services.AddValidatorsFromAssemblyContaining<CreatePatientDto>();
            services.AddValidatorsFromAssemblyContaining<UpdatePatientDto>();

            //Appointment Validator
            services.AddValidatorsFromAssemblyContaining<CreateAppointmentDto>();
            services.AddValidatorsFromAssemblyContaining<UpdateAppointmentDto>();
            services.AddValidatorsFromAssemblyContaining<RescheduleAppointmentDto>();

            //Authentication Validator
            services.AddValidatorsFromAssemblyContaining<RegisterDto>();
            services.AddValidatorsFromAssemblyContaining<LoginDto>();


            //Adding Automapper services
            services.AddAutoMapper(cfg => { }, typeof(MappingProfile));
            
            return services;
        }
    }
}
