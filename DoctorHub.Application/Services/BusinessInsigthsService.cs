using DoctorsHub.Application.DTOs.BusinessInsigts.AppointmentAnalyticsDto;
using DoctorsHub.Application.DTOs.BusinessInsigts.RevenueAnalyticsDto;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Interfaces.ServiceContracts;


namespace DoctorsHub.Application.Services
{
    public class BusinessInsigthsService : IBusinessInsightsService
    {
        //Private Feilds 
        private readonly IBusinessInsightsRepository _businessInsightsRepository;

        //Constructor
        public BusinessInsigthsService(IBusinessInsightsRepository businessInsightsRepository) 
        {
            _businessInsightsRepository = businessInsightsRepository;
        }

        public async Task<List<AppointmentsByDoctorDto>> GetAppointmentsByDoctorsAsync()
        {
            return await _businessInsightsRepository.GetAppointmentsByDoctorsAsync();
        }

        public async Task<List<AppointmentStatusChartDto>> GetAppointmentStatusChartAsync()
        {
            return await _businessInsightsRepository.GetAppointmentStatusChartAsync();
        }

        public async Task<List<AppointmentTrendDto>> GetAppointmentTrendsAsync()
        {
            return await _businessInsightsRepository.GetAppointmentTrendAsync();
        }

        public async Task<List<AverageRevenueGeneratedByDoctorDto>> GetAverageRevenueGeneratedByDoctorsAsync()
        {
            return await _businessInsightsRepository.GetAverageRevenueGeneratedByDoctors();
        }

        public async Task<List<PeakAppointmentHoursDto>> GetPeakAppointmentHoursAsync()
        {
            return await _businessInsightsRepository.GetPeakAppointmentHoursAsync();
        }

        public async Task<List<RevenueByDoctorDto>> GetRevenueByDoctorsAsync()
        {
            return await _businessInsightsRepository.GetRevenueByDoctorsAsync();
        }

        public async Task<List<RevenueTrendDto>> GetRevenueTrendsAsync()
        {
            return await _businessInsightsRepository.GetRevenueTrendAsync();
        }

        public async Task<List<TopRevenueGeneratingDoctorDto>> GetTopRevenueGeneratingDoctorsAsync()
        {
            return await _businessInsightsRepository.GetTopRevenueGeneratingDoctors();
        }
    }
}
