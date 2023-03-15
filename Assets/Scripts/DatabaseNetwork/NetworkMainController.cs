using Mirror;
using UnityEngine;

namespace DatabaseNetwork
{
    public class NetworkMainController : NetworkManager
    {
        private const string SERVER_ADDRESS = "80.80.109.15";
        private const string LOCALHOST_ADDRESS = "localhost";

        public override void Start()
        {
            networkAddress = LOCALHOST_ADDRESS;
#if UNITY_SERVER
             StartServer();          
             Debug.Log("Try Start server");

#else
            StartClient();
            Debug.Log("Try connect server");
#endif
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);
            Debug.Log($"Player joined server from {conn.address}");
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            Debug.Log("ServerStarted!");
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            Debug.Log("Connected to server");
        }
    }
}