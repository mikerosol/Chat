using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.BackgroundServices
{
    public class ChatMessageConsumerBackgroundService : IHostedService
    {
        private ChatMessageConsumer _chatMessageConsumer;

        public ChatMessageConsumerBackgroundService(ChatMessageConsumer chatMessageConsumer)
        {
            _chatMessageConsumer = chatMessageConsumer;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () => {
                await _chatMessageConsumer.ConsumeAsync();
            });
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
