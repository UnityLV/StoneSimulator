namespace GameState.Interfaces
{
    public interface IHealthDebugging
    {
        public int GetHealthPerLvl();
        public int GetHealthLvl(int lvl);
        public int GetHealth(int location, int lvl);
        public void SetHealth(int location, int lvl,int value);
        public void SetCurrentHealth(int value);
    }
}