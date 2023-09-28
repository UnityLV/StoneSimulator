using System.Threading.Tasks;
using FirebaseCustom.Loaders;
using FirebaseCustom;
using MongoDBCustom;
using Network;
using PlayerData;
using UnityEngine;

namespace Installers
{
    public class BootStraper : MonoBehaviour
    {
        [SerializeField] private CustomNetworkManager _customNetworkManager;

        [SerializeField] private MongoDBConnectionConfig _db;
        [SerializeField] private ConnectionConfig _mirror;
        [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private RankData _rankData;

        [SerializeField] private PlayerDataSetter _playerDataSetter;


        private IdbCommands _idbCommands;

        private async void Start()
        {
#if !UNITY_SERVER
            await LoadRemoteConfig();
            Debug.Log("RemoteConfig Loaded");
#endif


            await ConstructDB();

            await _playerDataSetter.SetData();
#if UNITY_SERVER
            await LoadLocationHealthFromDB();
#endif
            _customNetworkManager.TryConnect();
        }

        private async Task LoadLocationHealthFromDB()
        {
            int health = await _idbCommands.GetLocationHealth();
            ValuesFromBootScene.HealthPointPerLevel = health;
            Debug.Log( "HealthPointPerLevel: " + health);
        }

        private async Task ConstructDB()
        {
            var connection = await ConstructConnection();
            _idbCommands = new IdbCommands(connection);
            ValuesFromBootScene.IdbCommands = _idbCommands;
            ValuesFromBootScene.MongoConnection = connection;
        }

        private async Task<IMongoConnection> ConstructConnection()
        {
            IMongoConnection connection = new MongoDBConnect(_db).GetConnectionData();
            MongoDBConnector connector = new (connection);
            await connector.TryConnect();
            return connection;
        }

        private async Task LoadRemoteConfig()
        {
            RemoteConfigSetter remoteConfigSetter = new RemoteConfigSetter(_mirror, _db, playerConfig,_rankData);
            Config config = await new RemoteConfigLoader().Load();
            remoteConfigSetter.SetConfig(config);
        }
    }
}