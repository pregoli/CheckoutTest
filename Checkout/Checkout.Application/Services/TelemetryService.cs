using Checkout.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Checkout.Application.Services
{
    public class TelemetryService : ITelemetryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TelemetryService> _logger;

        public TelemetryService(
            HttpClient httpClient,
            ILogger<TelemetryService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetRequestsCount()
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/metrics/requests/count");
            return response.IsSuccessStatusCode ? 
                await response.Content.ReadAsStringAsync() : 
                response.ReasonPhrase;
        }
        
        public async Task<string> GetTopTenEventsAsync()
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/events/$all?$top=10");
            return response.IsSuccessStatusCode ? 
                await response.Content.ReadAsStringAsync() : 
                response.ReasonPhrase;
        }
    }
}
