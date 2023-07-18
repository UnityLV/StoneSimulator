using System;

namespace Stone.Interfaces
{
    public interface IStoneClickEvents
    {
        public event Action<int> OnStoneClick;
    }
}