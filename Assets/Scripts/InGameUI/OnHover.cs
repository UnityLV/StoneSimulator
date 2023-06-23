using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace InGameUI
{
    public class OnHover : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
    {
        [SerializeField] private UnityEvent _enter;  
        [SerializeField] private UnityEvent _exit;  
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _enter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _exit?.Invoke();
        }
    }
}