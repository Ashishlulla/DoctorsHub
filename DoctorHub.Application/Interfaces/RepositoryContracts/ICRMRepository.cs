using DoctorsHub.Application.DTOs.CRM;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface ICRMRepository
    {
        Task<DashBoardDto> GetDashBoardAsync();
    }
}
