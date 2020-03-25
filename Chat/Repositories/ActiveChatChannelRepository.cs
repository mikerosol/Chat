using System.Collections.Generic;

namespace Chat.Repositories
{
    public class ActiveChatChannelRepository
    {
        private List<string> _activeChatChannels { get; set; }

        public ActiveChatChannelRepository()
        {
            _activeChatChannels = new List<string>();
        }

        public List<string> GetAll()
        {
            return _activeChatChannels;
        }

        public void Upsert(string channel)
        {
            if (!_activeChatChannels.Contains(channel))
            {
                _activeChatChannels.Add(channel);
            }            
        }
    }
}
