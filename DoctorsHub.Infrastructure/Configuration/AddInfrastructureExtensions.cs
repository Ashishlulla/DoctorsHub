using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Application.Services;
using DoctorsHub.Infrastructure.Communication.Brevo;
using DoctorsHub.Infrastructure.Configuration;
using DoctorsHub.Infrastructure.Persistence;
using DoctorsHub.Infrastructure.Repositories;
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
            services.AddScoped<IBusinessInsightsRepository, BusinessInsightsRepository>();
            services.AddScoped<IBillingRepository, BillingRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            


            //Brevo settings 
            services.Configure<BrevoSettings>(configuration.GetSection("Brevo"));
            services.AddHttpClient<IEmailService, BrevoEmailService>(client =>
            {
                client.BaseAddress = new Uri("https://api.brevo.com/v3/");
            });


            return services;
        }
    }
}