using System;
using System.Collections.Generic;
using PlayerData.Interfaces;
using UnityEngine.Serialization;

namespace PlayerData
{
    [Serializable]
    public class SavablePlayerSlavesData
    {
       public List<SingleSlaveData> Slaves = new List<SingleSlaveData>();
     
    }
}