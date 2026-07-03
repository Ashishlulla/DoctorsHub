using DoctorHub.Application.DTOs.Doctors;
using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.DTOs.Patients;
using FluentValidation;

namespace DoctorsHub.Web.Configurations
{
    public static class ValidatorsExtension
    {
        public static IServiceCollection AddValidatorsService(this IServiceCollection services) 
        {
           
            //Doctor DTOs Validators
            services.AddValidatorsFromAssemblyContaining<CreateDoctorDto>();
            services.AddValidatorsFromAssemblyContaining<UpdateDoctorDto>();

            //Patient DTOs Validators
            services.AddValidatorsFromAssemblyContaining<CreatePatientDto>();
            services.AddValidatorsFromAssemblyContaining<UpdatePatientDto>();

            //Appointment DTOs Validators
            services.AddValidatorsFromAssemblyContaining<CreateAppointmentDto>();
            services.AddValidatorsFromAssemblyContaining<UpdateAppointmentDto>();
            services.AddValidatorsFromAssemblyContaining<RescheduleAppointmentDto>();

            return services;
        }
    }
}
