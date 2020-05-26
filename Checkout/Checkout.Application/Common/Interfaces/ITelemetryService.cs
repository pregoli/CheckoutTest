using System.Threading.Tasks;

namespace Checkout.Application.Common.Interfaces
{
    public interface ITelemetryService
    {
        Task<string> GetTopTenEventsAsync();
        Task<string> GetRequestsCount();
    }
}
