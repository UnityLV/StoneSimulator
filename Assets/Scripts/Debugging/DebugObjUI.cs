using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Debugging
{
    public class DebugObjUI:MonoBehaviour
    {
        public int LocationId;
        public int StoneId;
        
        [SerializeField]
        private Image _bgImage;

        [SerializeField]
        private TextMeshProUGUI _locationText;
        
        [SerializeField]
        private TextMeshProUGUI _stoneText;
        
        [SerializeField]
        private TextMeshProUGUI _hpText;

        public Action<int> OnEditEndAction;
        public Action OnBtnClickAction;

        public void SetLocationText(string value)
        {
            _locationText.text = "Location: " + value;
        }
        
        public void SetStoneText(string value)
        {
            _stoneText.text = "Stone: " + value;
        }
        
        public void SetHpText(string value)
        {
            _hpText.text = value;
        }

        public void OnBtnClick()
        {
            OnBtnClickAction?.Invoke();
        }
        
        public void OnEditEnd(string value)
        {
            string input = value;
            try
            {
                int result = Int32.Parse(input);
               OnEditEndAction?.Invoke(result);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse '{input}'");
            }
        }

        public void SetCurrentState(bool state)
        {
            _bgImage.color = state ? Color.green : Color.black;
            var cl = _bgImage.color;
            cl.a = 0.2f;
            _bgImage.color = cl;
        }

    }
}