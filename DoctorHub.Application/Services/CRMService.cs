using DoctorsHub.Application.DTOs.CRM;
using DoctorsHub.Application.Interfaces.ServiceContracts;


using System;
using System.Collections.Generic;
using System.Text;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using System.Diagnostics;
using AutoMapper;
using DoctorsHub.Domain.Entities;

namespace DoctorsHub.Application.Services
{
    public class CRMService : ICRMService
    {
        //Private Feilds
        private readonly ICRMRepository _crmRepository;
        private readonly IMapper _mapper;

        //Constructor
        public CRMService(ICRMRepository crmRepository, IMapper mapper) 
        {
            _crmRepository = crmRepository;
            _mapper = mapper;
        }

        public async Task<List<AppointmentStatusChartDto>> GetAppointmentStatusChartAsync()
        {
            return await _crmRepository.GetAppointmentsStatusChartAsync();
        }

        public async Task<DashBoardDto> GetDashBoardAsync()
        {
            return await _crmRepository.GetDashBoardAsync();
        }

        public async Task<List<RecentAppointmentsDto>> GetRecentAppointmentsAsync()
        {
            var data = await _crmRepository.GetRecentAppointmentsAsync();

            List<RecentAppointmentsDto> recentAppointments = _mapper.Map<List<RecentAppointmentsDto>>(data);

            return recentAppointments;
        }

        public async Task<List<ScheduledAppointmentsDto>> GetScheduledAppointmentsAsync()
        {
            var ScheduleAppointments = await _crmRepository.GetScheduledApointmentsAsync();

            return _mapper.Map<List<ScheduledAppointmentsDto>>(ScheduleAppointments);
        }

        public async Task<List<TodayAppointmentsDto>> GetTodaysAppointmentAsync()
        {
            var appointments = await _crmRepository.GetTodaysAppointmentsAsync();

            return _mapper.Map<List<TodayAppointmentsDto>>(appointments);
        }

        public async Task<List<UpcomingAppointmentsDto>> GetUpcomingAppointmentsAsync()
        {
            List<Appointment> appointments = await _crmRepository.GetUpcomingAppointmentsAsync();

            return _mapper.Map<List<UpcomingAppointmentsDto>>(appointments);
        }
    }
}
