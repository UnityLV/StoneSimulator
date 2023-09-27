using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace InGameUI
{
    public class ButtonsSettings : MonoBehaviour
    {
        [SerializeField] private float _fontSize;
        [SerializeField] private TMP_FontAsset _font;
        [SerializeField] private Color _fontColor;

        [Button]
        public void Set()
        {
            foreach (var button in GetComponentsInChildren<DayButton>())
            {
                button.GetComponentInChildren<TMP_Text>().fontSize = _fontSize;
                button.GetComponentInChildren<TMP_Text>().font = _font;
                button.GetComponentInChildren<TMP_Text>().color = _fontColor;
            }
        }
    }
}