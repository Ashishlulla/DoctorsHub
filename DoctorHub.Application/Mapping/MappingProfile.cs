using AutoMapper;
using DoctorHub.Application.DTOs.Doctors;
using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.DTOs.CRM;
using DoctorsHub.Application.DTOs.Patients;
using DoctorsHub.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            //Doctor Mapping
            CreateMap<CreateDoctorDto, Doctor>();
            CreateMap<UpdateDoctorDto, Doctor>();
            CreateMap<Doctor, DoctorDto>();

            //Patient Mapping
            CreateMap<CreatePatientDto, Patient>();
            CreateMap<UpdatePatientDto,Patient>();
            CreateMap<Patient, PatientDto>();
            CreateMap<Patient, UpdatePatientDto>();

            //Appointment Mapping
            CreateMap <CreateAppointmentDto, Appointment>();
            CreateMap<UpdateAppointmentDto, Appointment>();
            CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.DoctorName,
                opt => opt.MapFrom(src => src.Doctor.FullName))
            .ForMember(dest => dest.PatientName,
                opt => opt.MapFrom(src => src.Patient.FullName));
            
            CreateMap<Appointment, UpdateAppointmentDto>();
            CreateMap<Appointment, AppointmentDetailsDto>();
            CreateMap<AppointmentDto, AppointmentDetailsDto>();
            CreateMap<AppointmentDto, UpdateAppointmentDto>();

            //DashBoard Appointments Mapping
            CreateMap<Appointment, RecentAppointmentsDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName));

        }
    }
}
