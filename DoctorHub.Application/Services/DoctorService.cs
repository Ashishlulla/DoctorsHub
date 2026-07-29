using AutoMapper;
using DoctorHub.Application.DTOs.Doctors;
using DoctorsHub.Application.DTOs.common;
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
            // Generate base name
            var baseName = createDoctorDto.FullName
                .Replace(" ", "")
                .ToLower();


            // Generate unique email using name + birth date
            var emailBase =
                $"{baseName}{createDoctorDto.BirthDate.Day:D2}{createDoctorDto.BirthDate.Month:D2}";

            var email = $"{emailBase}@doctorhub.com";

            int counter = 1;

            while (await _userManager.FindByEmailAsync(email) != null)
            {
                email = $"{emailBase}{counter}@doctorhub.com";
                counter++;
            }


            // Generate temporary password
            var password = $"{baseName}@123";


            // Create Identity User
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };


            var result = await _userManager.CreateAsync(user, password);


            if (!result.Succeeded)
            {
                var errors = string.Join(", ",
                    result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(errors);
            }


            // Assign Doctor Role
            var roleResult = await _userManager.AddToRoleAsync(user, "Doctor");


            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ",
                    roleResult.Errors.Select(e => e.Description));

                await _userManager.DeleteAsync(user);

                throw new InvalidOperationException(errors);
            }


            // Create Doctor Profile
            var doctor = _mapper.Map<Doctor>(createDoctorDto);

            doctor.FullName = $"Dr. {createDoctorDto.FullName}";
            doctor.UserId = user.Id;


            doctor = await _doctorRepository.AddAsync(doctor);


            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task DeleteDoctorAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new KeyNotFoundException(
                    $"No doctor exists with Id = {id}");
            }


            // Delete Identity User
            if (!string.IsNullOrEmpty(doctor.UserId))
            {
                var user = await _userManager.FindByIdAsync(doctor.UserId);

                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);

                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ",
                            result.Errors.Select(e => e.Description));

                        throw new InvalidOperationException(errors);
                    }
                }
            }


            // Delete Doctor Profile
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
