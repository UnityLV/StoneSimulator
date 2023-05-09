using System;

namespace InGameUI.Interfaces
{
    public interface IInGameService
    {
        public void SetState(bool mainState, bool playState);
        public void SetOnHomeClickAction(Action action);
    }
}