namespace PlayerData.Interfaces
{
    public interface IClickDataService
    {
        public int GetClickCount();
        public void SetClickCount(int value);
        public void AddClick();
    }
}