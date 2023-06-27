using System.Threading.Tasks;
using FirebaseCustom;
using MongoDBCustom;
using Network;
using PlayerData;
using UnityEngine;
using UnityEngine.Events;

namespace Installers
{
    public class ChainLoader : MonoBehaviour
    {
        [SerializeField] private CustomNetworkManager _customNetworkManager;

        [SerializeField] private MongoDBConnectionConfig _db;
        [SerializeField] private ConnectionConfig _mirror;

        [SerializeField] private PlayerDataSetter _playerDataSetter;  

        public event UnityAction Loaded;  
        private DBValues _dbValues;

        private async void Start()
        {
            await LoadRemoteConfig();
            var connection = await ConstructConnection();

            _dbValues = new DBValues(connection);

            ValuesFromBootScene.DBValues = _dbValues;
            ValuesFromBootScene.MongoConnection = connection;

            await _playerDataSetter.SetData();
            
            _customNetworkManager.gameObject.SetActive(true);
        }

        private async Task<IMongoConnection> ConstructConnection()
        {
            IMongoConnection connection = new MongoDBConnectData(_db).GetConnectionData();
            MongoDBConnector connector = new MongoDBConnector(connection);
            await connector.TryConnect();
            return connection;
        }

        private async Task LoadRemoteConfig()
        {
            RemoteConfigSetter remoteConfigSetter = new RemoteConfigSetter(_mirror, _db);
            remoteConfigSetter.SetConfigs(await new RemoteConfigLoader().Load());
        }
    }
}