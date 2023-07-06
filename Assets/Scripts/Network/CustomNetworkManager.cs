using System;
using System.Threading.Tasks;
using GameScene;
using GameScene.Interfaces;
using Mirror;
using Network.Enum;
using Network.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Network
{
    public class CustomNetworkManager : NetworkManager, INetworkManagerService
    {
        [SerializeField] private FirebaseCustom.ConnectionConfig _connectionConfig; 
        
        #region Dependency

        private IGameSceneService _gameSceneService;

        [Inject]
        private void Construct(IGameSceneService gameSceneService)
        {
            _gameSceneService = gameSceneService;
        }

        #endregion


        private ConnectionType _currentConnectionType;
        
        private const string LOCALHOST_ADDRESS = "localhost";

        public override void Start()
        {
            //base.Start();
            
            TryConnect();
        }

        private void TryConnect()
        {
            networkAddress = _connectionConfig.Ip;
#if UNITY_SERVER
Debug.Log("Try start server");
            StartServer();

#else
            Debug.Log("Try connect server");
            StartClient();
#endif
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            _currentConnectionType = ConnectionType.Server;
            //_gameSceneService.BeginTransaction();
            NetworkServer.SpawnObjects();
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);
            Debug.Log("Player join on server");
            GameObject player = Instantiate(playerPrefab);
            DontDestroyOnLoad(player);
            NetworkServer.AddPlayerForConnection(conn, player);
        }

         public override async void OnClientDisconnect()
         {
             base.OnClientDisconnect();
             for (int i = 0; i < 5; i++) await Task.Yield();
             //TryConnect();
             //_gameSceneService.BeginTransaction();
             //_gameSceneService.BeginLoadGameScene(GameSceneType.Boot);
             //_gameSceneService.BeginTransaction();
         }
        
        public override async void OnClientConnect()
        {
            base.OnClientConnect();
            Debug.Log("Join on server");
            for (int i = 0; i < 160; i++) await Task.Yield();
            _currentConnectionType = ConnectionType.Client;
            //_gameSceneService.BeginTransaction();
        }

        //
        // public override void OnServerSceneChanged(string sceneName)
        // {
        //     base.OnServerSceneChanged(sceneName);
        //     Debug.Log("We are here " + sceneName);
        //     NetworkServer.SpawnObjects();
        // }
        public ConnectionType GetConnectionType()
        {
            return  _currentConnectionType;
        }

        public void SetPort(string port)
        {
            GetComponent<TelepathyTransport>().port = Convert.ToUInt16(port);
        }
    }
}