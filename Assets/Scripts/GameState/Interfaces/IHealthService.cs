namespace GameState.Interfaces
{
    public interface IHealthService
    {
        public int GetCurrentStoneHealth();
        public int GetCurrentLocationHealth();
        public int GetMaxLocationHealth();
        public int GetAllHealthInLocation(int location);
    }
}