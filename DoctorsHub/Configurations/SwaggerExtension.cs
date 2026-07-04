namespace DoctorsHub.Web.Configurations
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services) 
        {
            //services.AddOpenApi();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }
}
