using System;

namespace Stone.Interfaces
{
    public interface IStoneAnimatorEventCallback
    {
        public event Action OnStoneDestroyed;
        public event Action OnStoneClicked;
        public event Action OnStoneSpawned;
    }
}