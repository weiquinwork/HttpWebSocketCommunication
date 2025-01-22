using System.Net.WebSockets;
using System.Text;

namespace ClientProgram
{
    class Program
    {
        const string port = "5000";
        const string wsServerUrl = $"ws://localhost:{port}";
        const string httpServerUrl = $"http://localhost:{port}";

        static async Task Main(string[] args)
        {
            var serverUrl = $"{wsServerUrl}/messages";

            using var client = new ClientWebSocket();
            await client.ConnectAsync(new Uri(serverUrl), CancellationToken.None);

            using var httpClient = new HttpClient();

            var pingResponse = await httpClient.PostAsync($"{httpServerUrl}/server/ping", null);

            var startWorkResponse = httpClient.PostAsync($"{httpServerUrl}/work/start", null);

            while (client.State == WebSocketState.Open)
            {
                var message = await ReceiveMessageAsync(client);
                Console.WriteLine(message);
            }
        }

        static async Task<string> ReceiveMessageAsync(ClientWebSocket client)
        {
            var buffer = new byte[1024];
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            return Encoding.UTF8.GetString(buffer, 0, result.Count);
        }
    }
}
