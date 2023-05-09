using System;

namespace MainMenuUI.Inrefaces
{
    public interface IMainMenuService
    {
        public void SetState(bool state);
        public void SetOnCompleteLocationClickAction(Action<int> action);
        public void SetInProgressLocationClickAction(Action action);
    }
}