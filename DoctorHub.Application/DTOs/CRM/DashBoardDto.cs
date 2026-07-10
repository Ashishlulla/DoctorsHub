using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.CRM
{
    public class DashBoardDto
    {
        
        public int TotalDoctors { get; set; }
        
        public int TotalPatients { get; set; }
        
        public int TotalAppointments { get; set; }
        
        public int CompletedAppointments { get; set; }
        
        public int ScheduleAppointments { get; set; }
        
        public int CancelledAppointments { get; set; }
        
        public decimal TotalRevenue { get; set; }
        
        public double AverageDoctorRating { get; set; }
        
        public List<RecentAppointmentsDto> RecentAppointments { get; set; } = new List<RecentAppointmentsDto>();

        public List<UpcomingAppointmentsDto> UpcomingAppointments { get; set; } = new List<UpcomingAppointmentsDto>();

        public List<TodayAppointmentsDto> TodayAppointments { get; set; } = new List<TodayAppointmentsDto>();

        public List<ScheduledAppointmentsDto> ScheduledAppointmentsList { get; set; } = new List<ScheduledAppointmentsDto>();

        public List<AppointmentStatusChartDto> appointmentStatusChart { get; set; } = new List<AppointmentStatusChartDto>();

        public List<MonthlyAppointmentChartDto> monthlyAppointmentChart { get; set; } = new List<MonthlyAppointmentChartDto>();
    }
}
