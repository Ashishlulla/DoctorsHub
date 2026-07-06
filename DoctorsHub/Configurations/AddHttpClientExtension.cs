using DoctorHub.Application.Services;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Application.Services;

namespace DoctorsHub.Web.Configurations
{
    public static class AddHttpClientExtension
    {
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHttpClient<IPatientService, PatientService>(client =>
            {
                client.BaseAddress = new Uri(configuration["MyAPI:BaseUrl"]!);
            });

            services.AddHttpClient<IDoctorService, DoctorService>(client =>
            {
                client.BaseAddress = new Uri(configuration["MyAPI:BaseUrl"]!);
            });

            services.AddHttpClient<IAppointmentService, AppointmentService>(client =>
            {
                client.BaseAddress = new Uri(configuration["MyAPI:BaseUrl"]!);
            });


            return services;
        }
    }
}
