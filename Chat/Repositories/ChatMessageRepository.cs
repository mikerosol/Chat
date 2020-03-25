using System.Collections.Generic;
using System.Linq;
using Chat.Models;

namespace Chat.Repositories
{
    public class ChatMessageRepository
    {
        private List<ChatMessageModel> _messages { get; set; }

        public ChatMessageRepository()
        {
            _messages = new List<ChatMessageModel>();
        }

        public List<ChatMessageModel> GetByChannel(string channel)
        {
            return _messages
                .Where(x => x.Channel == channel)
                .ToList();
        }

        public void Add(ChatMessageModel chatMessageModel)
        {
            _messages.Add(chatMessageModel);
        }
    }
}
