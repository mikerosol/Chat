using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Chat.MessageHandlers;
using Chat.Messages;
using Chat.Repositories;

namespace Chat
{
    public class ChatMessageConsumer
    {
        private ChatChannelRepository _chatChannelRepository;
        private MessageSentMessageHandler _messageSentMessageHandler;

        private ConsumerConfig _consumerConfig { get; set; }
        private IConsumer<string, string> _consumer { get; set; }

        public ChatMessageConsumer(
            ChatChannelRepository chatChannelRepository, 
            MessageSentMessageHandler messageSentMessageHandler)
        {
            _chatChannelRepository = chatChannelRepository;
            _messageSentMessageHandler = messageSentMessageHandler;

            _consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = "localhost:29092",
                GroupId = "Chat",
                //GroupId = Guid.NewGuid().ToString(),
                AutoOffsetReset = AutoOffsetReset.Earliest,
                //MetadataMaxAgeMs = 1000,
                //TopicMetadataRefreshFastIntervalMs = 1000,
                //TopicMetadataRefreshIntervalMs = 1000

            };

            _consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();
        }

        public List<string> GetSubscribedTopics()
        {
            return _consumer.Subscription;
        }

        public void SubscribeToTopics(IEnumerable<string> channels)
        {
            _consumer.Subscribe(channels);
        }

        public async Task ConsumeAsync()
        {
            _consumer.Subscribe(_chatChannelRepository.GetAll());

            try
            {
                while (true)
                {
                    using (CancellationTokenSource cts = new CancellationTokenSource())
                    {
                        Console.CancelKeyPress += (_, e) =>
                        {
                            e.Cancel = true;
                            cts.Cancel();
                        };

                        try
                        {
                            var cr = _consumer.Consume(cts.Token);

                            var message = JsonConvert.DeserializeObject<ChatMessageSentMessage>(cr.Value);
                            Console.WriteLine(message.ToJsonString());
                            await _messageSentMessageHandler.Handle(message);                                  
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    };
                }
            }
            catch (OperationCanceledException)
            {
                _consumer.Close();
            }
        }
    }
}
