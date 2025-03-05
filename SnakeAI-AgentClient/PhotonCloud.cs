using Photon.Realtime;
using Microsoft.Extensions.Configuration;

namespace SnakeAI_AgentClient
{
    class PhotonCloud
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AgentClient: Initializing...");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config/appsettings.json")
                .Build();

            var photonSettings = configuration.GetSection("PhotonSettings");

            var appSettings = new AppSettings
            {
                AppIdRealtime = photonSettings["AppIdRealtime"],
                AppVersion = photonSettings["AppVersion"],
                FixedRegion = photonSettings["FixedRegion"],
                NetworkLogging = Enum.Parse<Photon.Client.LogLevel>(photonSettings["NetworkLogging"])
            };

            // Initialisation de Photon
            var loadBalancingClient = new RealtimeClient();
            loadBalancingClient.AddCallbackTarget(new ConnectionCallbacks());
            loadBalancingClient.ConnectUsingSettings(appSettings);

            Console.WriteLine("AgentClient: Connecting to Photon...");
            while (!loadBalancingClient.IsConnected)
            {
                loadBalancingClient.Service();
            }

            Console.WriteLine("AgentClient: Connected to Photon!");
        }
    }

    class ConnectionCallbacks : IConnectionCallbacks
    {
        public void OnConnected() => Console.WriteLine("AgentClient: OnConnected");
        public void OnConnectedToMaster() => Console.WriteLine("AgentClient: OnConnectedToMaster");
        public void OnDisconnected(DisconnectCause cause) => Console.WriteLine($"AgentClient: OnDisconnected - {cause}");
        public void OnRegionListReceived(RegionHandler regionHandler) => Console.WriteLine("AgentClient: OnRegionListReceived");
        public void OnCustomAuthenticationResponse(Dictionary<string, object> data) => Console.WriteLine("AgentClient: OnCustomAuthenticationResponse");
        public void OnCustomAuthenticationFailed(string debugMessage) => Console.WriteLine($"AgentClient: OnCustomAuthenticationFailed - {debugMessage}");
    }
}
