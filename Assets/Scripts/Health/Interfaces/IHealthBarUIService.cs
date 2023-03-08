namespace Health.Interfaces
{
    public interface IHealthBarUIService
    {
        public void UpdateHealthBarState(int currentHp, int maxHp);
    }
}