using System;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenuUI
{
    [RequireComponent(typeof(Button))]
    public class CopyLinkButton : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Share);
        }
        
        private void OnDestroy()
        {
            GetComponent<Button>().onClick.RemoveListener(Share);
        }

        private void Share()
        {
            UniClipboard.SetText (GetInviteLink());
        }

        private string GetInviteLink()
        {
            return ReferralLinkGenerator.GetLink();
        }
    }
}