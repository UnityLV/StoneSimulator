using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace ChatDB.PinMessage
{
    public class PinMessageTextInput : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private RectTransform _inputUI;
        public event UnityAction<string> GetMessage;

        private void OnEnable()
        {
            _confirmButton.onClick.AddListener(OnConfirm);
        }

        private void OnDisable()
        {
            _confirmButton.onClick.RemoveListener(OnConfirm);
        }

        public void Show()
        {
            _inputUI.gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            _inputUI.gameObject.SetActive(false);
        }

        private void OnConfirm()
        {
            if (string.IsNullOrEmpty(_inputField.text) == false)
            {
                string message = _inputField.text;
                GetMessage?.Invoke(message);
            }
        }
    }
}