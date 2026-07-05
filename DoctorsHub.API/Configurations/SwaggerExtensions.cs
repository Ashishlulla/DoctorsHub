using Microsoft.AspNetCore.Http.HttpResults;

namespace DoctorsHub.API.Configurations
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services) 
        {

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }
}
