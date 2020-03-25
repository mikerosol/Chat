using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Chat.Models;
using Chat.Repositories;
using Chat.ViewModels;

namespace Chat.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ChatChannelRepository _chatChannelRepository;
        private readonly ChatMessageRepository _chatMessageRepository;

        public HomeController(
            ILogger<HomeController> logger, 
            ChatChannelRepository chatChannelRepository,
            ChatMessageRepository chatMessageRepository)
        {
            _logger = logger;
            _chatChannelRepository = chatChannelRepository;
            _chatMessageRepository = chatMessageRepository;
        }

        public IActionResult Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return View("Index", new ChatChannelsViewModel()
                {
                    ChatChannels = _chatChannelRepository.GetAll()
                });
            }

            else
            {
                return View("Channel", new ChatChannelViewModel()
                {
                    Channel = id,
                    Messages = _chatMessageRepository.GetByChannel(id)
                });             
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
