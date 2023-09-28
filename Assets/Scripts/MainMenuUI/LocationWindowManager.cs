using System;
using InGameUI;
using UnityEngine;

namespace MainMenuUI
{
    public class LocationWindowManager : MonoBehaviour
    {
        [SerializeField] private LocationWindowDataConstructor _dataConstructor;
        [SerializeField] private CompleteLocationWindow _window;

        private Action<int> _onCLickCallback;
        private int _selectedLocation;

        private void Awake()
        {
            _window.EnterButtonClicked += OnEnterButtonClicked;
        }

        private void OnDestroy()
        {
            _window.EnterButtonClicked -= OnEnterButtonClicked;
        }

        public void ShowWindow(int level, Action<int> onCLickCallback)
        {
            Debug.Log("Show Enter Complete Location Window");

            OpenWindow();

            _selectedLocation = level;
            _onCLickCallback = onCLickCallback;

            InitWindow(level);
        }

        private void InitWindow(int level)
        {
            _dataConstructor.SetDataFor(_window, level);
        }

        private void OnEnterButtonClicked()
        {
            _onCLickCallback?.Invoke(_selectedLocation);
            HideWindow();
        }

        private void OpenWindow()
        {
            _window.gameObject.SetActive(true);
        }

        private void HideWindow()
        {
            _window.gameObject.SetActive(false);
        }
    }
}