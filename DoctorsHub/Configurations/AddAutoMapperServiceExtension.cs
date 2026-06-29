using DoctorsHub.Application.Mapping;
using AutoMapper;

namespace DoctorsHub.Web.Configurations
{
    public static class AddAutoMapperServiceExtension
    {
        public static IServiceCollection AddAutoMapperServices(this IServiceCollection services) 
        {
            services.AddAutoMapper(cfg =>{ }, typeof(MappingProfile));
            return services;
        }
    }
}
