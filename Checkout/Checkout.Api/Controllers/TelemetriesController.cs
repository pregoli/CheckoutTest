using System.Threading.Tasks;
using Checkout.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class TelemetriesController : ControllerBase
    {
        private readonly ITelemetryService _telemetryService;

        public TelemetriesController(ITelemetryService telemetryService)
        {
            _telemetryService = telemetryService;
        }

        // GET: api/beta/Telemetries/events/all/top/10
        [HttpGet("beta/[controller]/events/all/top/10", Name = nameof(GetLastTenEvents))]
        public async Task<ActionResult<string>> GetLastTenEvents()
        {
            return await _telemetryService.GetTopTenEventsAsync();
        }

        // GET: api/beta/Telemetries/requests/count
        [HttpGet("beta/[controller]/requests/count", Name = nameof(GetRequestsCount))]
        public async Task<ActionResult<string>> GetRequestsCount()
        {
            return await _telemetryService.GetRequestsCount();
        }
    }
}
