using System;

namespace GameState.Interfaces
{
    public interface IGameStateCallbacks
    {
        public event Action OnHealthChanged;
        public event Action OnLocationChanged;
    }
}