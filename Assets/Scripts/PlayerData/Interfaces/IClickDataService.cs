using UnityEngine.Events;

namespace PlayerData.Interfaces
{
    public interface IClickDataService
    {
        event UnityAction<int> ClickUpdated;
        public int GetClickCount();
        public int GetAllClickCount();
        public void SetClickCount(int value);
        public void AddClick();
    }
}