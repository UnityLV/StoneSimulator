using System.Collections.Generic;
using PlayerData.Interfaces;
using UnityEngine;

namespace InGameUI
{
    public class SlaveLines : MonoBehaviour
    {
        [SerializeField] private SlaveSingleLine _linePrefab;

        public void SetData(IEnumerable<SingleSlaveData> data)
        {
            foreach (var slaveData in data)
            {
                Instantiate(_linePrefab, transform).SetData(slaveData);
            }
        }
    }
}