using System;
using System.Collections.Generic;
using InGameUI;
using MongoDBCustom;
using PlayerData.Interfaces;
using UnityEngine;
using Zenject;

namespace MainMenuUI
{
    public class SlaveDataConstructor : MonoBehaviour
    {
        [SerializeField] private SlaveUI _slaveUI;
        private ISlavesDataService _slavesData;

        [Inject]
        private void Construct(ISlavesDataService slavesDataService)
        {
            _slavesData = slavesDataService;
        }

        private async void OnEnable()
        {
            SlavesData Data = _slavesData.GetSlaves();
            _slaveUI.SetData(Data);
        }

    }
}