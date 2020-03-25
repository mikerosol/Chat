using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chat.Repositories;

namespace Chat.BackgroundServices
{
    public class ChatChannelBackgroundService : IHostedService
    {
        private ChatMessageConsumer _chatMessageConsumer;
        private ActiveChatChannelRepository _activeChatChannelRepository;

        public ChatChannelBackgroundService(
            ChatMessageConsumer chatMessageConsumer,
            ActiveChatChannelRepository activeChatChannelRepository)
        {
            _chatMessageConsumer = chatMessageConsumer;
            _activeChatChannelRepository = activeChatChannelRepository;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => {
                while (true)
                {
                    // GET ALL CURRENTLY USED CHANNELS
                    var channels = _activeChatChannelRepository.GetAll();

                    //GET CURRENT TOPICS
                    var currentTopics = _chatMessageConsumer.GetSubscribedTopics();

                    //SUBSCRIPT TO ALL CURRENLY USED CHANNELS
                    if (channels.Any() && (!currentTopics.SequenceEqual(channels)))
                    {

                        _chatMessageConsumer.SubscribeToTopics(channels);
                    }

                    Thread.Sleep(1000);
                }
                
            });

        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
