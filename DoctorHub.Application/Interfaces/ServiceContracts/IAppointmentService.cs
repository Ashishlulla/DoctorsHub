using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface IAppointmentService
    {
        Task CreateAppointmentAsync(CreateAppointmentDto createAppointmentDto);

        Task<List<AppointmentDto>> GetAppointmentsAsync();
        
        Task<AppointmentDto> GetAppointmentByIdAsync(int id);

        Task<(List<AppointmentDto> Appointments, int TotalRecords)> GetAllAppointmentsAsync(AppointmentQueryParameter appointmentQueryParameter);

        Task<UpdateAppointmentDto> GetAppointmentForUpdateByIdAsync(int id);

        Task<AppointmentDetailsDto> GetAppointmentForDetailsByIdAsync(int id);
        Task UpdateAppointmentAsync(UpdateAppointmentDto updateAppointmentDto);

        Task DeleteAppointmentAsync(int id);

        Task ConfirmedAppointmentAsync(int appointmentId);

        Task RescheduleAppointmentAsync(RescheduleAppointmentDto rescheduleAppointmentDto);

        Task CancelAppointmentAsync(int appointmentId);

        Task CompletedAppointmentAsync(int appoinmentId);
    }
}
