using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Models
{
    public class ChatMessageModel
    {
        public string Channel { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}
