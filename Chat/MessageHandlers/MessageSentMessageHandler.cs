using System.Threading.Tasks;
using Chat.Handlers;
using Chat.Messages;
using Chat.Models;
using Chat.Repositories;

namespace Chat.MessageHandlers
{
    public class MessageSentMessageHandler
    {
        private ChatHandler _chatHandler;
        private ChatMessageRepository _chatMessageRepository;

        public MessageSentMessageHandler(
            ChatHandler chatHandler, 
            ChatMessageRepository chatMessageRepository)
        {
            _chatHandler = chatHandler;
            _chatMessageRepository = chatMessageRepository;
        }

        public async Task Handle(ChatMessageSentMessage chatMessageSentMessage)
        {
            await _chatHandler.InvokeClientMethodToAllAsync(
                chatMessageSentMessage.Channel, 
                chatMessageSentMessage.UserId,
                chatMessageSentMessage.Message);

            _chatMessageRepository.Add(new ChatMessageModel()
            {
                Channel = chatMessageSentMessage.Channel,
                UserId = chatMessageSentMessage.UserId,
                Message = chatMessageSentMessage.Message
            });
        }
    }
}
