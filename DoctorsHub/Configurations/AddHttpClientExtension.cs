using DoctorHub.Application.Services;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Application.Services;

namespace DoctorsHub.Web.Configurations
{
    public static class AddHttpClientExtension
    {
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
        {

            services.AddHttpClient<IPatientService, PatientService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5025/");
            });

            services.AddHttpClient<IDoctorService, DoctorService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5025/");
            });

            services.AddHttpClient<IAppointmentService, AppointmentService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5025/");
            });




            return services;
        }
    }
}
