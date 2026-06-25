using DoctorHub.Application.DTOs.Doctors;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorHub.Application.Interfaces
{
    public interface IDoctorService
    {
        Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto? createDoctorDto);
    }
}
