using DoctorsHub.Application.DTOs.Billing;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;

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
        public async Task<PagedResult<BillDto>> GetFilteredBillAsync(BillingQueryParameter billingQueryParameter) 
        {

            string url =
            $"/api/Billing/filtered?" +
            $"searchBy={billingQueryParameter.searchBy ?? string.Empty}" +
            $"&searchString={billingQueryParameter.searchString ?? string.Empty}" +
            $"&sortBy={billingQueryParameter.sortBy ?? string.Empty}" +
            $"&sortOrder={(billingQueryParameter.sortOrder ?? string.Empty)}" +
            $"&PageSize={billingQueryParameter.PageSize}" +
            $"&PageNumber={billingQueryParameter.PageNumber}";


            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }


            PagedResult<BillDto>? bills = await response.Content.ReadFromJsonAsync<PagedResult<BillDto>>();

            return bills ?? new PagedResult<BillDto>();
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
