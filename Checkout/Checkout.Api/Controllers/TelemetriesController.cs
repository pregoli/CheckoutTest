using System.Threading.Tasks;
using Checkout.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelemetriesController : ControllerBase
    {
        private readonly ITelemetryService _telemetryService;

        public TelemetriesController(ITelemetryService telemetryService)
        {
            _telemetryService = telemetryService;
        }

        // GET: beta/events/all/top/10
        [HttpGet("beta/events/all/top/10")]
        public async Task<ActionResult<string>> Get()
        {
            return await _telemetryService.GetTopTenEventsAsync();
        }
    }
}
