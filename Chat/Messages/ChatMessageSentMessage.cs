using Chat.Models;

namespace Chat.Messages
{
    public class ChatMessageSentMessage : KafkaMessage
    {
        public string UserId;
        public string Channel;
        public string Message;

        public ChatMessageSentMessage()
        {
            MetaData.MessageType = MessageTypeEnum.ChatMessageSentMessage;
        }
    }
}
