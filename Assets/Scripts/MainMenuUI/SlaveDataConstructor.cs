using System;
using InGameUI;
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

        private void Start()
        {
            SlavesData Data = _slavesData.GetSlaves();
            _slaveUI.SetData(Data);
        }

    }
}