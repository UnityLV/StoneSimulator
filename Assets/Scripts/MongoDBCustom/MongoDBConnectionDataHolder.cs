using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace MongoDBCustom
{
    public class MongoDBConnectionDataHolder : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onGetData; 

        private IDBConnectionProvider _idbConnectionProvider;

        [Inject]
        private void Construct(IDBConnectionProvider idbConnectionProvider)
        {
            _idbConnectionProvider = idbConnectionProvider;
            _idbConnectionProvider.SuccessConnect += OnSuccessConnect;
        }

        public static MongoDBConnectionData Data { get; private set; }

        private void OnDisable()
        {
            _idbConnectionProvider.SuccessConnect -= OnSuccessConnect;
        }

        private void OnSuccessConnect(MongoDBConnectionData data)
        {
            Data = data;
            _onGetData?.Invoke();
        }
    }
}