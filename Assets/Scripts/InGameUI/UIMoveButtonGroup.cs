using NaughtyAttributes;
using UnityEngine;

namespace InGameUI
{
    public class UIMoveButtonGroup : MonoBehaviour
    {
        [SerializeField] private UIMoveButton[] _buttons;
  
        
        [Button()]
        public void Move()
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].Move();
            }
           
        }
    }
}