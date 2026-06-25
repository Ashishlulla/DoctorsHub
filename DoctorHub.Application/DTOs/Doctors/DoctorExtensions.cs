using DoctorHub.Application.DTOs.Doctors;
using DoctorsHub.Domain.Entities;

namespace DoctorsHub.Application.DTOs.Doctors
{
    public static class DoctorExtensions
    {
        public static Doctor ToDoctor(this CreateDoctorDto dto, string userId)
        {
            return new Doctor
            {
                UserId = userId,
                FullName = dto.FullName,
                Qualification = dto.Qualification,
                About = dto.About,
                ExperienceYears = dto.ExperienceYears,
                ConsultationFee = dto.ConsultationFee,
                VisitDays = dto.VisitDays,
                SpecializationId = dto.SpecializationId
            };
        }

        public static DoctorDto ToDoctorDto(
            this Doctor doctor,
            string email)
        {
            return new DoctorDto
            {
                Id = doctor.Id,
                FullName = doctor.FullName,
                Email = email,
                Qualification = doctor.Qualification,
                ExperienceYears = doctor.ExperienceYears,
                ConsultationFee = doctor.ConsultationFee,
                AverageRating = doctor.AverageRating,
                SpecializationName = doctor.Specialization?.Name ?? string.Empty
            };
        }
    }
}

