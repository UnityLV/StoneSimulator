using System;
using PlayerData.Interfaces;
using Stone.Interfaces;
using UnityEngine.Events;


namespace PlayerData
{
    public class PlayerClickData : BasePlayerData<ClickData>, IClickDataService
    {
        private const string CLICK_DATA_PATCH = "ClickData";
        public event UnityAction<int> ClickUpdated;

        public PlayerClickData() : base(CLICK_DATA_PATCH)
        {
            Load();
        }

        public int GetClickCount()
        {
            return Data.ClickCount;
        }

        public int GetAllClickCount()
        {
            return Data.AllClickCount;
        }

        public void ResetAll()
        {
            Data.ClickCount = 0;
            Data.AllClickCount = 0;
            Save();
        }

        public void SetClickCount(int value)
        {
            Data.ClickCount = value;
            Data.AllClickCount = value;
            Save();
        }

        public void AddClicks(int amount = 1)
        {
            Data.ClickCount += amount;
            Data.AllClickCount += amount;
            Save();
            ClickUpdated?.Invoke(Data.ClickCount);
        }
    }
}