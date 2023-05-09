using UnityEngine;

namespace LocationGameObjects.Interfaces
{
    public interface IGetLocationSpritesService
    {
        public Sprite GetBGLocationSprite(int location);
        public Sprite GetAvatarLocationSprite(int location);
    }
}