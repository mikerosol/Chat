using System.Collections.Generic;

namespace Chat.Repositories
{
    public class ChatChannelRepository
    {
        public List<string> GetAll()
        {
            return new List<string>()
            {
                "Accounting", 
                "Development", 
                "General",
                "HR", 
                "Marketing",
                "Production",
                "Support"
            };
        }
    }
}
