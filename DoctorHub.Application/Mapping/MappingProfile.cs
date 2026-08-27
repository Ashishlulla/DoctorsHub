using AutoMapper;
using DoctorsHub.Application.DTOs.Doctors;
using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.DTOs.Billing;
using DoctorsHub.Application.DTOs.CRM;
using DoctorsHub.Application.DTOs.Patients;
using DoctorsHub.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using DoctorsHub.Application.DTOs.Notification;
using DoctorsHub.Application.DTOs.Departments;

namespace DoctorsHub.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            // Doctor Mapping

            CreateMap<CreateDoctorDto, Doctor>()
                .ForMember(dest => dest.Departments, opt => opt.Ignore());

            CreateMap<UpdateDoctorDto, Doctor>()
                .ForMember(dest => dest.Departments, opt => opt.Ignore());

            CreateMap<Doctor, DoctorDto>()
                .ForMember(
                    dest => dest.Email,
                    opt => opt.MapFrom(src => src.User!.Email))
                .ForMember(
                    dest => dest.DepartmentIds,
                    opt => opt.MapFrom(src =>
                        src.Departments.Select(d => d.Id)));

            //Patient Mapping
            CreateMap<CreatePatientDto, Patient>();
            CreateMap<UpdatePatientDto,Patient>();
            CreateMap<Patient, PatientDto>();
            CreateMap<Patient, UpdatePatientDto>();
            CreateMap<PatientDto, UpdatePatientDto>();

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

            CreateMap<Appointment, UpcomingAppointmentsDto>()
                .ForMember(dest => dest.DoctorName,
                opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName));
            
            CreateMap<Appointment, TodayAppointmentsDto>()
                .ForMember(dest => dest.DoctorName,
                opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName));

            CreateMap<Appointment, ScheduledAppointmentsDto>()
                .ForMember(dest => dest.DoctorName,
                opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName));

            //Billing DTOs Mapping
            CreateMap<Bill, BillDto>()
                .ForMember(dest=>dest.PaymentStatus,
                opt=>opt.MapFrom(src=>src.PaymentStatus.ToString()))
                .ForMember(dest=>dest.DoctorName, 
                opt=>opt.MapFrom(src=>src.Appointment.Doctor.FullName))
                .ForMember(dest => dest.PatientName,
                opt => opt.MapFrom(src => src.Appointment.Patient.FullName)); //converts Bill to billdto
            

            CreateMap<CreateBillDto, Bill>()
                .ForMember(dest=>dest.ConsultationFee, opt=>opt.MapFrom(src=>src.ConsultationFee)); //converts CreateBilldto to bill

            CreateMap<UpdateBillDto, Bill>(); //converts UpdateBilldto to bill
            CreateMap<Bill, UpdateBillDto>(); //converts Bill to updatebilldto
            CreateMap<BillDto, UpdateBillDto>();

            //Notification Mapping

            CreateMap<Notification, NotificationDto>(); //converts Notification to NotificationDto


            //Department Mapping
            CreateMap<Department, DepartmentDto>(); //Converts Departments to DepartmentDto

            CreateMap<CreateDepartmentDto, Department>(); // Converts CreateDepartmentDto to Department

            CreateMap<UpdateDepartmentDto, Department>(); //Convertts UpdateDepartmentDto to Department



        }
    }
}
