using AutoMapper;
using DoctorHub.Application.DTOs.Doctors;
using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.DTOs.Patients;
using DoctorsHub.Domain.Entities;
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
            CreateMap<Appointment, AppointmentDto>();
            CreateMap<Appointment, UpdateAppointmentDto>();
        }
    }
}
