namespace GameState.Interfaces
{
    public interface IGameStateService
    {
        public void TryStartGame();
        public void TryWatchLocation(int id);
    }
}