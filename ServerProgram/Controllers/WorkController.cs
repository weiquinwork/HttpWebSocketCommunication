using Microsoft.AspNetCore.Mvc;

namespace ServerProgram.Controllers
{
    [ApiController]
    [Route("work")]
    public class WorkController : ControllerBase
    {
        private readonly WebSocketService _webSocketService;

        public WorkController(WebSocketService webSocketService)
        {
            _webSocketService = webSocketService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartWork()
        {
            var workId = Guid.NewGuid().ToString();
            await _webSocketService.SendToAllAsync($"WorkStarted: {workId}");

            // Simulate work
            await Task.Delay(2000);

            await _webSocketService.SendToAllAsync($"WorkCompleted: {workId}");
            return Ok();
        }
    }
}
