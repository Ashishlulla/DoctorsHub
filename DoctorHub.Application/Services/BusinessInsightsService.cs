using DoctorsHub.Application.DTOs.BusinessInsigts;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Domain.Enums;


namespace DoctorsHub.Application.Services
{
    public class BusinessInsightsService : IBusinessInsightsService
    {
        //Private Feilds 
        private readonly IBusinessInsightsRepository _businessInsightsRepository;

        //Constructor
        public BusinessInsightsService(IBusinessInsightsRepository businessInsightsRepository) 
        {
            _businessInsightsRepository = businessInsightsRepository;
        }

        public  async Task<BusinessInsightsDto> GetBusinessInsightsAsync(AnalyticsTimeFilter timeFilter)
        {
            return await _businessInsightsRepository.GetBusinessInsightsAsync(timeFilter);
        }
    }
}
