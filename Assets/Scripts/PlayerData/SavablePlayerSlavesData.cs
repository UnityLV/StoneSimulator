using System;
using System.Collections.Generic;
using PlayerData.Interfaces;

namespace PlayerData
{
    [Serializable]
    public class SavablePlayerSlavesData
    {
        public List<SingleSlaveData> Data = new List<SingleSlaveData>();
    }
}