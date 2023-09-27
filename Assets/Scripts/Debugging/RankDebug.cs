using Installers;
using MongoDBCustom;
using NaughtyAttributes;
using UnityEngine;

namespace Debugging
{
    public class RankDebug : MonoBehaviour
    {
        private IDBCommands IdbCommands => ValuesFromBootScene.IdbCommands;

        [Button()]
        private async void Test()
        {
            var data = await IdbCommands.GetPlayerDataAsync();
            Debug.Log(data);
        }
    }
}