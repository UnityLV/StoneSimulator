using NativeElements.Scripts;
using UnityEngine;
namespace MainMenuUI
{
    public class ShareReferralLinkButton : MonoBehaviour
    {
        private string ReferralLink => ReferralLinkGenerator.GetLink();
        
        public void Share()
        {
            ShareMessage.Share(ReferralLink);
        }
    }
}