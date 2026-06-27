using DoctorHub.Application.DTOs.Doctors;
using FluentValidation;

namespace DoctorsHub.Web.Configurations
{
    public static class ValidatorsExtension
    {
        public static IServiceCollection AddValidatorsService(this IServiceCollection services) 
        {
           
            services.AddValidatorsFromAssemblyContaining<CreateDoctorDto>();

            return services;
        }
    }
}
