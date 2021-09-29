using Kafka.Domain.Models.Callback;
using System.Threading.Tasks;

namespace Kafka.Application.Services.Callback
{
    public interface ICallbackService
    {
        Task<SendCallbackResponse> SetCallback(SendCallbackRequest sendCallbackRequest);
    }
}
