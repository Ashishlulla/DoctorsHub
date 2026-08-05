using DoctorsHub.Web.Services;

namespace DoctorsHub.Web.Configurations
{
    public static class AddExportServiceExtension
    {
        public static IServiceCollection AddExportService(this IServiceCollection services)
        {
            services.AddScoped<ExcelExportService>();
            services.AddScoped<PdfExportService>();
            return services;
        }
    }
}
