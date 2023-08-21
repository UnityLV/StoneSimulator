using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace GameScene
{
    public class WatchADWindow : MonoBehaviour
    {
        [SerializeField] private Button _watchButton;
        [SerializeField] private Button _sendInviteLinkButton;
        [SerializeField] private Button _copyInviteLinkButton;

        [SerializeField] private UnityEvent _onWatchButtonClick;
        [SerializeField] private UnityEvent _onSendInviteLinkButtonClick;
        [SerializeField] private UnityEvent _onCopyInviteLinkButtonClick;

        private void Awake()
        {
            _watchButton.onClick.AddListener(OnWatchButtonClick);
            _sendInviteLinkButton.onClick.AddListener(OnSendInviteLinkButtonClick);
            _copyInviteLinkButton.onClick.AddListener(OnCopyInviteLinkButtonClick);
        }

        private void OnDestroy()
        {
            _watchButton.onClick.RemoveListener(OnWatchButtonClick);
            _sendInviteLinkButton.onClick.RemoveListener(OnSendInviteLinkButtonClick);
            _copyInviteLinkButton.onClick.RemoveListener(OnCopyInviteLinkButtonClick);
        }

        private void OnSendInviteLinkButtonClick()
        {
            _onSendInviteLinkButtonClick?.Invoke();
        }

        private void OnWatchButtonClick()
        {
            _onWatchButtonClick?.Invoke();
        }

        private void OnCopyInviteLinkButtonClick()
        {
            _onCopyInviteLinkButtonClick?.Invoke();
        }
    }
}