using Photon.Realtime;
using Microsoft.Extensions.Configuration;

namespace SnakeAI_AdminClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AdminClient: Initializing...");

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

            Console.WriteLine("AdminClient: Connecting to Photon...");
            while (!loadBalancingClient.IsConnected)
            {
                loadBalancingClient.Service();
            }

            Console.WriteLine("AdminClient: Connected to Photon!");
        }
    }

    class ConnectionCallbacks : IConnectionCallbacks
    {
        public void OnConnected() => Console.WriteLine("AdminClient: OnConnected");
        public void OnConnectedToMaster() => Console.WriteLine("AdminClient: OnConnectedToMaster");
        public void OnDisconnected(DisconnectCause cause) => Console.WriteLine($"AdminClient: OnDisconnected - {cause}");
        public void OnRegionListReceived(RegionHandler regionHandler) => Console.WriteLine("AdminClient: OnRegionListReceived");
        public void OnCustomAuthenticationResponse(Dictionary<string, object> data) => Console.WriteLine("AdminClient: OnCustomAuthenticationResponse");
        public void OnCustomAuthenticationFailed(string debugMessage) => Console.WriteLine($"AdminClient: OnCustomAuthenticationFailed - {debugMessage}");
    }
}