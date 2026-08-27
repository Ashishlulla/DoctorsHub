using AutoMapper;
using DoctorsHub.Application.DTOs.Doctors;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using DoctorsHub.Application.DTOs.Communication;


namespace DoctorsHub.Application.Services
{
    public class DoctorService : IDoctorService
    {
        //Private Feilds
        private readonly IDoctorRepository _doctorRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        private readonly IEmailService _emailService;


        //Constructor
        public DoctorService(IDoctorRepository doctorRepository, IDepartmentRepository departmentRepository, UserManager<ApplicationUser> userManager, IMapper mapper, IEmailService emailService)
        {
            _doctorRepository = doctorRepository;
            _departmentRepository = departmentRepository;
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
        }
        public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto createDoctorDto)
        {
            // Generate base name
            var baseName = createDoctorDto.FullName
                .Replace(" ", "")
                .ToLower();


            // Generate unique email using name + birth date (Professional email For DoctorsHub Login)
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
                PersonalEmail = createDoctorDto.PersonalEmail,
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

            var selectedDepartments = new List<Department>();

            foreach (var departmentId in createDoctorDto.DepartmentIds.Distinct()) 
            {

                var department = await _departmentRepository.GetDepartmentByIdAsync(departmentId);

                if (department == null)
                    throw new KeyNotFoundException($"Department with Id {departmentId} was not found.");

                selectedDepartments.Add(department);
            }

            doctor.Departments = selectedDepartments;


            doctor = await _doctorRepository.AddAsync(doctor);

            //Sending system generated  email and temporary system generated login credentials newly joined doctor on their PersonalEmail
            await _emailService.SendAsync(
                new EmailMessageDto
                {
                    To = doctor.PersonalEmail,
                    ToName = doctor.FullName,
                    Subject = "DoctorsHub Temporary Login Credentials",

                    HtmlBody = $"""
                    <h2>Welcome to DoctorsHub</h2>

                    <p>
                        We are pleased to welcome you to DoctorsHub.
                        Please find your temporary login credentials below.
                    </p>

                    <br>

                    <h4>Login Credentials</h4>

                    <p><strong>DoctorsHub Email:</strong> {doctor.User.Email}</p>
                    <p><strong>Temporary Password:</strong> {password}</p>

                    <p>
                        <strong>Please Note:</strong>
                        These are temporary login credentials.
                        Please change your password after your first successful login.
                    </p>
                    """,

                    PlainTextBody = $"""
                    Welcome to DoctorsHub

                    We are pleased to welcome you to DoctorsHub.
                    Please find your temporary login credentials below.

                    Login Credentials
                    DoctorsHub Email: {doctor.User.Email}
                    Temporary Password: {password}

                    Please Note:
                    These are temporary login credentials.
                    Please change your password after your first successful login.
                    """
                });

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

            doctor.Departments.Clear();

            var selectedDepartments = new List<Department>();

            foreach (var departmentId in updateDoctorDto.DepartmentIds)
            {
                var department = await _departmentRepository.GetDepartmentByIdAsync(departmentId);

                if (department == null)
                {
                    throw new KeyNotFoundException($"No department found with departmentId : {departmentId}");
                }
                selectedDepartments.Add(department);
            }

            doctor.Departments = selectedDepartments;


            _mapper.Map(updateDoctorDto, doctor);

            await _doctorRepository.UpdateDoctorAsync(doctor);
        }
    }
}
