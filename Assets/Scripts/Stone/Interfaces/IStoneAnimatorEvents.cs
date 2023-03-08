using System;

namespace Stone.Interfaces
{
    public interface IStoneAnimatorEvents
    {
        public event Action OnStoneDestroyPlay;
        public event Action OnStoneClickPlay;
        public event Action OnStoneSpawnPlay;
    }
}