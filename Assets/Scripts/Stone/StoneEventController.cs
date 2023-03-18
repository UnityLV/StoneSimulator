using System;
using Stone.Interfaces;

namespace Stone
{
    public class StoneEventController : IStoneClickEvents, IStoneClickEventsInvoke, IStoneAnimatorEvents,
        IStoneAnimatorEventsInvoke, IStoneAnimatorEventCallback, IStoneAnimatorCallbackInvoke
    {
        
        public event Action OnStoneClick;

        public void OnStoneClickInvoke()
        {
            OnStoneClick?.Invoke();
        }

        public event Action OnStoneDestroyPlay;
        public event Action OnStoneClickPlay;
        public event Action OnStoneSpawnPlay;

        public void OnStoneDestroyPlayInvoke()
        {
            OnStoneDestroyPlay?.Invoke();
        }

        public void OnStoneClickPlayInvoke()
        {
            OnStoneClickPlay?.Invoke();
        }

        public void OnStoneSpawnPlayInvoke()
        {
            OnStoneSpawnPlay?.Invoke();
        }

        public event Action OnStoneDestroyed;
        public event Action OnStoneClicked;
        public event Action OnStoneSpawned;

        public void OnStoneDestroyedInvoke()
        {
            OnStoneDestroyed?.Invoke();
        }

        public void OnStoneClickedInvoke()
        {
            OnStoneClicked?.Invoke();
        }

        public void OnStoneSpawnedInvoke()
        {
            OnStoneSpawned?.Invoke();
        }
    }
}