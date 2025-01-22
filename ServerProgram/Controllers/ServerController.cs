using Microsoft.AspNetCore.Mvc;

namespace ServerProgram.Controllers
{
    [ApiController]
    [Route("server")]
    public class ServerController : ControllerBase
    {
        private readonly WebSocketService _webSocketService;

        public ServerController(WebSocketService webSocketService)
        {
            _webSocketService = webSocketService;
        }

        [HttpPost("ping")]
        public async Task<IActionResult> Ping()
        {
            await _webSocketService.SendToAllAsync("Pong");
            return Ok();
        }
    }
}
