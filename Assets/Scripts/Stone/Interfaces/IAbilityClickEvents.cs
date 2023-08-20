using System;
namespace Stone.Interfaces
{
    public interface IAbilityClickEvents
    {
        public event Action<int> OnAbilityClick;
    }
}