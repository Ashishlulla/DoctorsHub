using DoctorHub.Application.Services;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Application.Services;
using DoctorsHub.Web.Services;

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


            services.AddHttpClient<DoctorApiService>(client =>
            {
                client.BaseAddress = new Uri(configuration["MyAPI:BaseUrl"]!);
            });

            services.AddHttpClient<SpecializationApiService>(client =>
            {
                client.BaseAddress = new Uri(configuration["MyAPI:BaseUrl"]!);
            });

            services.AddHttpClient<PatientApiService>(client => 
            {
                client.BaseAddress = new Uri(configuration["MyAPI:BaseUrl"]!);
            });

            services.AddHttpClient<AppointmentApiService>(client =>
            {
                client.BaseAddress = new Uri(configuration["MyAPI:BaseUrl"]!);
            });


            return services;
        }
    }
}
