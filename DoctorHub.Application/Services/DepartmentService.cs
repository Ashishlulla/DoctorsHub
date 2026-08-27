

using AutoMapper;
using DoctorsHub.Application.DTOs.Departments;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Domain.Entities;

namespace DoctorsHub.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        //Private Feilds
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        //Constructor
        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper) 
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<DepartmentDto> AddDepartmentAsync(CreateDepartmentDto createDepartmentDto)
        {
            Department department = _mapper.Map<Department>(createDepartmentDto);

            Department result = await _departmentRepository.CreateDepartmentAsync(department);

            return _mapper.Map<DepartmentDto>(result);
        }

        public async Task DeleteDepartmentAsync(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Department Id cannot be null or empty. Please provide valid Id");
            }

            Department? department = await _departmentRepository.GetDepartmentByIdAsync(id.Value);

            if (department == null)
            {
                throw new ArgumentException($"No department exist with id = {id}. Please provide valid Id.");
            }

            await _departmentRepository.DeleteDepartmentAsync(department);
        }

        public async Task<DepartmentDto> GetDepartmentByIdAsync(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id),"Department Id cannot be null or empty. Please provide valid Id");
            }

            Department? department = await _departmentRepository.GetDepartmentByIdAsync(id.Value);

            if (department == null)
            {
                throw new ArgumentException($"No department exist with id = {id}. Please provide valid Id.");
            }

            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDetailsDto> GetDepartmentDetailsAsync(int id)
        {
            Department? department =
                await _departmentRepository.GetDepartmentDetailsAsync(id);

            if (department == null)
            {
                throw new ArgumentException(
                    $"No department exists with id = {id}. Please provide valid Id.");
            }

            return new DepartmentDetailsDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,

                Doctors = department.Doctors
                    .Select(d => new DepartmentDoctorDto
                    {
                        Id = d.Id,
                        FullName = d.FullName,
                        Email = d.User?.Email ?? string.Empty,
                        Qualification = d.Qualification,
                        ExperienceYears = d.ExperienceYears,
                        ConsultationFee = d.ConsultationFee,
                        SpecializationName = d.Specialization?.Name ?? string.Empty
                    })
                    .ToList()
            };
        }

        public async Task<List<DepartmentDto>> GetDepartmentsAsync()
        {
            List<Department> departments = await _departmentRepository.GetDepartmentsAsync();

            return _mapper.Map<List<DepartmentDto>>(departments);
        }

        public async Task<DepartmentDto> UpdateDepartmentAsync(UpdateDepartmentDto updateDepartmentDto)
        {
            Department? department = await _departmentRepository.GetDepartmentByIdAsync(updateDepartmentDto.Id);

            if (department == null)
            {
                throw new ArgumentException($"No department exist with id = {updateDepartmentDto.Id}. Please provide valid Id.");
            }

            department.Name = updateDepartmentDto.Name;
            department.Description = updateDepartmentDto.Description;

            Department updatedDepartment = await _departmentRepository.UpdateDepartmentAsync(department);

            return _mapper.Map<DepartmentDto>(updatedDepartment);
        }
    }
}
