using DoctorsHub.Application.DTOs.Appoitments;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface IAppointmentService
    {
        Task CreateAppointmentAsync(CreateAppointmentDto createAppointmentDto);

        Task<List<AppointmentDto>> GetAllAppointmentsAsync();

        Task<AppointmentDto> GetAppointmentByIdAsync(int id);

        Task<UpdateAppointmentDto> GetAppointmentForUpdateByIdAsync(int id);

        Task<AppointmentDetailsDto> GetAppointmentForDetailsByIdAsync(int id);
        Task UpdateAppointmentAsync(UpdateAppointmentDto updateAppointmentDto);

        Task DeleteAppointmentAsync(int id);
    }
}
