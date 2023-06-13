﻿using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace MongoDBCustom
{
    public class MongoDBDataHolder : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onGetData; 

        private IDBProvider _dbProvider;

        [Inject]
        private void Construct(IDBProvider dbProvider)
        {
            _dbProvider = dbProvider;
            _dbProvider.SuccessConnect += OnSuccessConnect;
        }

        public static MongoDBConnectionData Data;


        private void OnDisable()
        {
            _dbProvider.SuccessConnect -= OnSuccessConnect;
        }

        private void OnSuccessConnect(MongoDBConnectionData data)
        {
            Data = data;
            _onGetData?.Invoke();
        }
    }
}