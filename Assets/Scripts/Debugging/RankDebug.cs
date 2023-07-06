using System;
using Installers;
using MongoDBCustom;
using NaughtyAttributes;
using PlayerData.Interfaces;
using UnityEngine;
using Zenject;

namespace Debugging
{
    public class RankDebug : MonoBehaviour
    {
        private IDBValues _dbValues => ValuesFromBootScene.DBValues;

        [Button()]
        private async void Test()
        {
            var data = await _dbValues.GetPlayerDataAsync();
            Debug.Log(data);
        }
    }
}