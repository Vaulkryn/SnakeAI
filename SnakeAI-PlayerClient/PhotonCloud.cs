using Photon.Realtime;
using Microsoft.Extensions.Configuration;

namespace SnakeAI_PlayerClient
{
    class PhotonCloud
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PlayerClient: Initializing...");

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

            Console.WriteLine("PlayerClient: Connecting to Photon...");
            while (!loadBalancingClient.IsConnected)
            {
                loadBalancingClient.Service();
            }

            Console.WriteLine("PlayerClient: Connected to Photon!");
        }
    }

    class ConnectionCallbacks : IConnectionCallbacks
    {
        public void OnConnected() => Console.WriteLine("PlayerClient: OnConnected");
        public void OnConnectedToMaster() => Console.WriteLine("PlayerClient: OnConnectedToMaster");
        public void OnDisconnected(DisconnectCause cause) => Console.WriteLine($"PlayerClient: OnDisconnected - {cause}");
        public void OnRegionListReceived(RegionHandler regionHandler) => Console.WriteLine("PlayerClient: OnRegionListReceived");
        public void OnCustomAuthenticationResponse(Dictionary<string, object> data) => Console.WriteLine("PlayerClient: OnCustomAuthenticationResponse");
        public void OnCustomAuthenticationFailed(string debugMessage) => Console.WriteLine($"PlayerClient: OnCustomAuthenticationFailed - {debugMessage}");
    }
}