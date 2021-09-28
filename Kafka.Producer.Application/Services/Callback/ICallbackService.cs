using Kafka.Producer.Domain.Models.Callback;
using System.Threading.Tasks;

namespace Kafka.Producer.Application.Services.Callback
{
    public interface ICallbackService
    {
        Task<SendCallbackResponse> SetCallback(SendCallbackRequest sendCallbackRequest);
    }
}
