using DoctorsHub.Application.DTOs.BusinessInsigts;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Services
{
    public class BusinessInsigthsService :IBusinessInsightsService
    {
        //Private Feilds 
        private readonly IBusinessInsightsRepository _businessInsightsRepository;

        //Constructor
        public BusinessInsigthsService(IBusinessInsightsRepository businessInsightsRepository) 
        {
            _businessInsightsRepository = businessInsightsRepository;
        }

        public async Task<List<AppointmentStatusChartDto>> GetAppointmentStatusChart()
        {
            return await _businessInsightsRepository.GetAppointmentStatusChartAsync();
        }
    }
}
