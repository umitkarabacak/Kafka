using Kafka.Domain.Models.Callback;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kafka.Application.Services.Callback
{
    public class CallbackService : ICallbackService
    {
        private readonly ILogger<CallbackService> _logger;

        public CallbackService()
        {

        }

        public CallbackService(ILogger<CallbackService> logger)
        {
            _logger = logger;
        }

        public async Task<SendCallbackResponse> SetCallback(SendCallbackRequest sendCallbackRequest)
        {
            _logger?.LogInformation("Set Callback\t" + JsonSerializer.Serialize(sendCallbackRequest));

            return await Task.FromResult(
                new SendCallbackResponse()
            );
        }
    }
}
