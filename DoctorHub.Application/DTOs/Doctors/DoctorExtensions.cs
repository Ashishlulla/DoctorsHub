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
                PhoneNumber = dto.PhoneNumber,
                Qualification = dto.Qualification,
                About = dto.About,
                ExperienceYears = dto.ExperienceYears,
                ConsultationFee = dto.ConsultationFee,
                VisitDays = dto.VisitDays,
                SpecializationId = dto.SpecializationId
            };
        }

        public static DoctorDto ToDoctorDto(this Doctor doctor)
        {
            return new DoctorDto
            {
                Id = doctor.Id,
                FullName = doctor.FullName,
                Email = doctor.User?.Email ?? string.Empty,
                PhoneNumber = doctor.PhoneNumber,
                Qualification = doctor.Qualification,
                ExperienceYears = doctor.ExperienceYears,
                ConsultationFee = doctor.ConsultationFee,
                AverageRating = doctor.AverageRating,
                SpecializationId = doctor.SpecializationId,
                VisitDays = doctor.VisitDays,
                About =  doctor.About,
                SpecializationName = doctor.Specialization?.Name ?? string.Empty
            };
        }
        public static UpdateDoctorDto ToUpdateDoctorDto(this Doctor doctor)
        {
            return new UpdateDoctorDto
            {
                FullName = doctor.FullName,
                PhoneNumber = doctor.PhoneNumber,
                Qualification = doctor.Qualification,
                About = doctor.About,
                ExperienceYears = doctor.ExperienceYears,
                ConsultationFee = doctor.ConsultationFee,
                VisitDays = doctor.VisitDays,
                SpecializationId = doctor.SpecializationId
            };
        }


        public static void UpdateDoctor(this Doctor doctor, UpdateDoctorDto dto)
        {
            doctor.FullName = dto.FullName;
            doctor.PhoneNumber = dto.PhoneNumber;
            doctor.Qualification = dto.Qualification;
            doctor.About = dto.About;
            doctor.ExperienceYears = dto.ExperienceYears;
            doctor.ConsultationFee = dto.ConsultationFee;
            doctor.VisitDays = dto.VisitDays;
            doctor.SpecializationId = dto.SpecializationId;
        }
    }
}