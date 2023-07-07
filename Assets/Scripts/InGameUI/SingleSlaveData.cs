using System;
using UnityEngine.Serialization;

namespace PlayerData.Interfaces
{
    [Serializable]
    public struct SingleSlaveData
    {
        public string Name;
        public int Clicks;
        public string DeviseId;
    }
}