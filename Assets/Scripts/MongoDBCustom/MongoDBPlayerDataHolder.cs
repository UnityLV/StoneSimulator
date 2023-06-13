using MongoDB.Bson;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace MongoDBCustom
{
    public class MongoDBPlayerDataHolder : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onLoadData;

        private IDBPlayerDataProvider _playerDataProvider;

        [Inject]
        private void Construct(IDBPlayerDataProvider playerDataProvider)
        {
            _playerDataProvider = playerDataProvider;
        }

        public static BsonDocument PlayerData { get; private set; }

        public async void LoadPlayerData()
        {
            PlayerData = await _playerDataProvider.GetPlayerDataByIdAsync();
            _onLoadData?.Invoke();
            Debug.Log(PlayerData.ToString());
        }
    }
}