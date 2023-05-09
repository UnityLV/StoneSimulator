using System.Collections.Generic;

namespace MainMenuUI.LocationMainMenu.Interfaces
{
    public interface ILocationFactoryService
    {
        public List<LocationMainMenuObject> CreateLocationObjects(int count);
    }
}