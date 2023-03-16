using Mirror;
using UnityEngine;

namespace Network
{
    public class CustomNetworkManager : NetworkManager
    {
        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);
            Debug.Log("Player join on server");
            GameObject player = Instantiate(playerPrefab);
            DontDestroyOnLoad(player);
            NetworkServer.AddPlayerForConnection(conn, player);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            base.OnServerSceneChanged(sceneName);
            Debug.Log("We are here " + sceneName);
            NetworkServer.SpawnObjects();
        }
    }
}