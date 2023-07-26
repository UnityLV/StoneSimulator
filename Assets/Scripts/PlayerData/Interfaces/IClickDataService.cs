using UnityEngine.Events;

namespace PlayerData.Interfaces
{
    public interface IClickDataService
    {
        event UnityAction<int> ClickUpdated;
        public int GetClickCount();
        public int GetAllClickCount();
        public void ResetAll();
        public void SetClickCount(int value);
        public void AddClicks(int amount = 1);
    }
}