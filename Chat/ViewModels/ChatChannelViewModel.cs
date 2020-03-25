using System.Collections.Generic;
using Chat.Models;

namespace Chat.ViewModels
{
    public class ChatChannelViewModel
    {
        public string Channel { get; set; }
        public List<ChatMessageModel> Messages { get; set; }
    }
}
