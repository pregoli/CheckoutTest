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

        public async Task<string> GetTopTenEventsAsync()
        {
            var response = _httpClient.GetAsync(_httpClient.BaseAddress).Result;
            return response.IsSuccessStatusCode ? 
                response.Content.ReadAsStringAsync().Result : 
                response.ReasonPhrase;
        }
    }
}
