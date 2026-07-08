using DoctorsHub.Application.DTOs.CRM;
using DoctorsHub.Application.Interfaces.ServiceContracts;


using System;
using System.Collections.Generic;
using System.Text;
using DoctorsHub.Application.Interfaces.RepositoryContracts;

namespace DoctorsHub.Application.Services
{
    public class CRMService : ICRMService
    {
        //Private Feilds
        private readonly ICRMRepository _crmRepository;

        //Constructor
        public CRMService(ICRMRepository crmRepository) 
        {
            _crmRepository = crmRepository;
        }
        public async Task<DashBoardDto> GetDashBoardAsync()
        {
            return await _crmRepository.GetDashBoardAsync();
        }
    }
}
