using DoctorsHub.Application.DTOs.Billing;

namespace DoctorsHub.Web.Services
{
    public class BillingApiService
    {
        //Private Fields
        private readonly HttpClient _httpClient;

        //Constructor
        public BillingApiService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }


        public async Task<IEnumerable<BillDto>> GetBillsAsync() 
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/Billing");
            response.EnsureSuccessStatusCode();

            IEnumerable<BillDto>? bills = await response.Content.ReadFromJsonAsync<IEnumerable<BillDto>>();

            return bills ?? new List<BillDto>();
        }

        public async Task<BillDto> GetBillByIdAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Billing/{id}");
            response.EnsureSuccessStatusCode();

            BillDto? bill = await response.Content.ReadFromJsonAsync<BillDto>();

            return bill ?? new BillDto ();
        }

        public async Task<BillDto> GetBillByAppointmentIdAsync(int appointmentId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Billing/appointment/{appointmentId}");
            response.EnsureSuccessStatusCode();

            BillDto? bill = await response.Content.ReadFromJsonAsync<BillDto>();

            return bill ?? new BillDto();
        }

        public async Task UpdateBillAsync(int id, UpdateBillDto updateBillDto)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"/api/Billing/{id}", updateBillDto);
            response.EnsureSuccessStatusCode();

            
        }
    }
}
