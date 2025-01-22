using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerProgram
{
    public class WebSocketService
    {
        private readonly List<WebSocket> _connectedSockets = new List<WebSocket>();

        public async Task HandleWebSocketAsync(WebSocket webSocket)
        {
            _connectedSockets.Add(webSocket);
            await SendToSocketAsync(webSocket, "Welcome");

            var buffer = new byte[1024];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    _connectedSockets.Remove(webSocket);
                }
            }
        }

        public async Task SendToAllAsync(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            var buffer = new ArraySegment<byte>(bytes);

            foreach (var socket in _connectedSockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        private async Task SendToSocketAsync(WebSocket socket, string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            var buffer = new ArraySegment<byte>(bytes);
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
