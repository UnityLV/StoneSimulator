using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenuUI
{
    public struct LocationWindowData
    {
        public string _clicksText;
        public string _locationName;
        public int _levelFrom;
        public Sprite _locationImage;

        public LocationWindowData(Sprite locationImage, int levelFrom, string locationName, string clicksText)
        {
            _locationImage = locationImage;
            _levelFrom = levelFrom;
            _locationName = locationName;
            _clicksText = clicksText;
        }
    }

    public class CompleteLocationWindow : MonoBehaviour
    {
        [SerializeField] private Image _locationImage;
        [SerializeField] private TMP_Text _levelFromText;
        [SerializeField] private TMP_Text _levelToText;

        [SerializeField] private TMP_Text _locationName;
        [SerializeField] private TMP_Text _clicksText;

        [SerializeField] private Button _enterButton;
        public event Action EnterButtonClicked;
        
        [SerializeField] private UnityEvent _onEnabled;
        [SerializeField] private UnityEvent _onDisabled;

        public void SetData(LocationWindowData locationWindowData)
        {
            _locationImage.sprite = locationWindowData._locationImage;
            _levelFromText.text = "Lv." + locationWindowData._levelFrom.ToString();
            _levelToText.text = "Lv." + (locationWindowData._levelFrom + 1).ToString();
            _locationName.text = locationWindowData._locationName;
            _clicksText.text = locationWindowData._clicksText;
        }

        private void Awake()
        {
            _enterButton.onClick.AddListener(OnEnterButtonClick);
        }
        
        private void OnEnable()
        {
            _onEnabled?.Invoke();
        }
        
        private void OnDisable()
        {
            _onDisabled?.Invoke();
        }

        private void OnDestroy()
        {
            _enterButton.onClick.RemoveListener(OnEnterButtonClick);
        }

        private void OnEnterButtonClick()
        {
            EnterButtonClicked?.Invoke();
        }
    }
}