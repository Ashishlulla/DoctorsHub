using DoctorsHub.Application.DTOs.Doctors;
using DoctorsHub.Application.DTOs.Patients;
using DoctorsHub.Application.DTOs.Reports.AppointmentsReport;

using DoctorsHub.Application.DTOs.Reports.BillingReport;

namespace DoctorsHub.Web.Services
{
    public class ReportsApiService
    {
        //Private Feilds
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Constructor
        public ReportsApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddToken()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["JWT"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<DoctorDto>> GetDoctorsAsync()
        {
            AddToken();
            HttpResponseMessage response = await _httpClient.GetAsync("api/doctors/all");
            response.EnsureSuccessStatusCode();
            List<DoctorDto> doctors = await response.Content.ReadFromJsonAsync<List<DoctorDto>>();
            return doctors ?? new List<DoctorDto>();
        }

        public async Task<List<PatientDto>> GetPatientsAsync()
        {
            AddToken();
            HttpResponseMessage response = await _httpClient.GetAsync("api/patients/all");
            response.EnsureSuccessStatusCode();
            List<PatientDto>? patients = await response.Content.ReadFromJsonAsync<List<PatientDto>>();
            return patients ?? new List<PatientDto>();
        }

        public async Task<List<AppointmentReportDto>> GetAppointmentReportsAsync(AppointmentReportFilteredDto appointmentReportFiltered)
        {
            string status= appointmentReportFiltered.Status.HasValue ? appointmentReportFiltered.Status.Value.ToString() : "null";
            AddToken();
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/reports/appointments", appointmentReportFiltered);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to retrieve appointment reports. {error}");
            }

            List<AppointmentReportDto>? AppointmentReports = await response.Content.ReadFromJsonAsync<List<AppointmentReportDto>>();
            
            return AppointmentReports ?? new List<AppointmentReportDto>();

        }

        public async Task<List<BillingReportDto>> GetBillingReportsAsync(BillingReportFilterDto billingReportFilter)
        {
            
            AddToken();
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/reports/billing", billingReportFilter);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to retrieve billing report. {error}");
            }

            List<BillingReportDto>? BillingReports = await response.Content.ReadFromJsonAsync<List<BillingReportDto>>();

            return BillingReports ?? new List<BillingReportDto>();

        }
    }
}
