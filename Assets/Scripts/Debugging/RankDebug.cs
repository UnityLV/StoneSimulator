using System;
using NaughtyAttributes;
using PlayerData.Interfaces;
using UnityEngine;
using Zenject;

namespace Debugging
{
    public class RankDebug : MonoBehaviour
    {
        private IRankDataService _rankDataService;

        [Inject]
        private void Construct(IRankDataService rankDataService)
        {
            _rankDataService = rankDataService;
        }


        [Button()]
        private void Test()
        {
            Debug.Log(_rankDataService.GetMyRank().ToString());
        }
    }
}