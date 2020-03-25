using System.Threading.Tasks;
using Chat.Messages;
using Chat.Producers;
using Chat.Repositories;
using WebSocketManager;

namespace Chat.Handlers
{
    public class ChatHandler : WebSocketHandler
    {
        private KafkaProducer _kafkaProducer;
        private ActiveChatChannelRepository _activeChatChannelRepository;

        public ChatHandler(
            WebSocketConnectionManager webSocketConnectionManager, 
            KafkaProducer kafkaProducer,
            ActiveChatChannelRepository chatMessageRepository) : base(webSocketConnectionManager)
        {
            _kafkaProducer = kafkaProducer;
            _activeChatChannelRepository = chatMessageRepository;
        }

        public async Task SendMessage(string socketId, string channel, string message)
        {
            //SUBSCRIPT TO ALL CURRENLY USED CHANNELS
            //_activeChatChannelRepository.Upsert(channel);

            //PRODUCE KAFKA MESSAGE
            await _kafkaProducer.ProduceAsync(new ChatMessageSentMessage()
            {
                UserId = socketId,
                Channel = channel,
                Message = message
            }, channel, 1);
        }
    }
}
