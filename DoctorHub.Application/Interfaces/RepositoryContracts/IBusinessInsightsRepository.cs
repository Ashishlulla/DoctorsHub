using DoctorsHub.Application.DTOs.BusinessInsigts;

using DoctorsHub.Domain.Enums;


namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface IBusinessInsightsRepository
    {
        Task<BusinessInsightsDto> GetBusinessInsightsAsync(AnalyticsTimeFilter timeFilter);
    }
}
