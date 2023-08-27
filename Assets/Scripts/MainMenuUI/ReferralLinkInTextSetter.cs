using TMPro;
using UnityEngine;
namespace MainMenuUI
{
    public class ReferralLinkInTextSetter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private void Awake()
        {
            _text.text = ReferralLinkGenerator.GetLink();
        }
    }
}