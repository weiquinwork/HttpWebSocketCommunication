using Microsoft.AspNetCore.Mvc;

namespace ServerProgram.Controllers
{
    [ApiController]
    [Route("messages")]
    public class WebSocketController : ControllerBase
    {
        private readonly WebSocketService _webSocketService;

        public WebSocketController(WebSocketService webSocketService)
        {
            _webSocketService = webSocketService;
        }

        [HttpGet]
        public async Task AcceptWebSocketConnection()
        {
            if (HttpContext.Request.Headers["Upgrade"] != "websocket")
            {
                HttpContext.Response.StatusCode = 400;
                return;
            }

            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await _webSocketService.HandleWebSocketAsync(webSocket);
        }
    }
}
