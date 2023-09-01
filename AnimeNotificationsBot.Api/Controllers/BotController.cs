using AnimeNotificationsBot.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace AnimeNotificationsBot.Api.Controllers
{
    
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly IBotService _botService;

        public BotController(IBotService botService)
        {
            _botService = botService;
        }

        [Route("bot")]
        [HttpPost]
        public async Task<IActionResult> Bot([FromBody] Update update, CancellationToken cancellationToken)
        {
            await _botService.HandleUpdateAsync(update, cancellationToken);
            return Ok();
        }
    }
}
