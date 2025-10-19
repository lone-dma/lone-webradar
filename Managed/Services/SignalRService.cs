using LoneWebRadar.Managed.Data;
using LoneWebRadar.Managed.MessagePack;
using MessagePack;
using Microsoft.AspNetCore.SignalR.Client;

namespace LoneWebRadar.Managed.Services
{
    public sealed class SignalRService
    {
        private HubConnection _hubConnection;
        private IDisposable _hubReg;

        /// <summary>
        /// Current SignalR connection state.
        /// </summary>
        public HubConnectionState ConnectionState => _hubConnection?.State ?? HubConnectionState.Disconnected;

        /// <summary>
        /// Latest Radar Data.
        /// </summary>
        public WebRadarUpdate Data { get; private set; }

        private Task OnRadarUpdate(WebRadarUpdate data)
        {
            if (data.Version < this.Data?.Version)
                return Task.CompletedTask;
            this.Data = data;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Start the SignalR connection.
        /// </summary>
        /// <param name="host">IP/Hostname to connect to.</param>
        /// <param name="szPort">Port to connect to (in string format).</param>
        /// <param name="password">Password used to authenticate with server.</param>
        /// <returns>Task on completion.</returns>
        public async Task StartConnectionAsync(string host, string szPort, string password)
        {
            // Stop existing connection (if any)
            await StopConnectionAsync();

            // Setup new connection
            int port = int.Parse(szPort.Trim());
            var remoteHost = new Uri($"http://{Utils.FormatIPForURL(host)}:{port}/hub/0f908ff7-e614-6a93-60a3-cee36c9cea91?password={Uri.EscapeDataString(password)}");

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(remoteHost)
                .WithAutomaticReconnect()
                .AddMessagePackProtocol(options =>
                {
                    options.SerializerOptions = MessagePackSerializerOptions.Standard
                        .WithSecurity(MessagePackSecurity.TrustedData)
                        .WithCompression(MessagePackCompression.Lz4BlockArray)
                        .WithResolver(ResolverGenerator.Instance);
                })
                .Build();
            _hubReg = _hubConnection.On<WebRadarUpdate>(
                "RadarUpdate", OnRadarUpdate);

            // Begin connection
            await _hubConnection.StartAsync();
        }

        /// <summary>
        /// Stop the SignalR connection.
        /// </summary>
        /// <returns></returns>
        public async Task StopConnectionAsync()
        {
            await (_hubConnection?.DisposeAsync() ?? ValueTask.CompletedTask);
            _hubConnection = null;
            _hubReg?.Dispose();
            _hubReg = null;
            this.Data = null;
        }
    }
}
