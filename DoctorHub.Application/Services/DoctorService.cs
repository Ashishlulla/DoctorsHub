using AutoMapper;
using DoctorHub.Application.DTOs.Doctors;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.Doctors;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Domain.Identity;
using Microsoft.AspNetCore.Identity;


namespace DoctorHub.Application.Services
{
    public class DoctorService : IDoctorService
    {
        //Private Feilds
        private readonly IDoctorRepository _doctorRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;


        //Constructor
        public DoctorService(IDoctorRepository doctorRepository, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto createDoctorDto)
        {
            var existingEmail = await _userManager.FindByEmailAsync(createDoctorDto.Email);

            if (existingEmail != null)
            {
                throw new InvalidOperationException($"Doctor with Email: {createDoctorDto.Email}  already Exists.");
            }

            var user = new ApplicationUser
            {
                UserName = createDoctorDto.Email,
                Email = createDoctorDto.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, createDoctorDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException(errors);
            }
            var roleResult = await _userManager.AddToRoleAsync(user, "Doctor");

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            }
            var doctor = _mapper.Map<Doctor>(createDoctorDto);
            doctor.UserId= user.Id;

            doctor = await _doctorRepository.AddAsync(doctor);

            return _mapper.Map<DoctorDto>(doctor); 
         }

        public async Task DeleteDoctorAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                throw new KeyNotFoundException($"No doctor exists with Id ={id}");
            }

            await _doctorRepository.DeleteDoctorByIdAsync(id);
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetDoctorsAsync();

            return _mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<(List<DoctorDto> Data, int TotalCount)> GetAllDoctorsAsync(DoctorQueryParameters doctorQueryParameters)
        {
            var (doctors, TotalRecords) = await _doctorRepository.GetAllDoctorsAsync(doctorQueryParameters);

            var doctlist = _mapper.Map<List<DoctorDto>>(doctors);

            return (doctlist, TotalRecords);
        }

        public async Task<DoctorDto?> GetByIdAsync(int id)
        {
            var doctor = await  _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new KeyNotFoundException($"No doctor exists with Id ={id}");
            }

            return _mapper.Map<DoctorDto>(doctor);
        }

        

        public async Task UpdateDoctorAsync(int id, UpdateDoctorDto updateDoctorDto)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new KeyNotFoundException($"No doctor exists with Id: {id}");
            }



            _mapper.Map(updateDoctorDto, doctor);

            await _doctorRepository.UpdateDoctorAsync(doctor);
        }
    }
}
