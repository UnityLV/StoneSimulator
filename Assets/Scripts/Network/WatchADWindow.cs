using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameScene
{
    public class WatchADWindow : MonoBehaviour
    {
        [SerializeField] private Button _watchButton;

        [SerializeField] private UnityEvent _onWatchButtonClick;
 
        private void Awake()
        {
            _watchButton.onClick.AddListener(OnWatchButtonClick);
        }

        private void OnDestroy()
        {
            _watchButton.onClick.RemoveListener(OnWatchButtonClick);
        }

        private void OnWatchButtonClick()
        {
            _onWatchButtonClick?.Invoke();
        }
    }
}