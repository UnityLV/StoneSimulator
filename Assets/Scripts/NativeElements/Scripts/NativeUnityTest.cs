using NativeAndroidElements;
using UnityEngine;
namespace NativeElements.Scripts
{
    public class NativeUnityTest : MonoBehaviour
    {
        public void ShowAlert()
        {
            Alert alertDiaolog = new Alert();
            alertDiaolog.onFail += () => { Toast.Show("adding function to delegate", Toast.LENGTH_LONG); };
            alertDiaolog.onFailLitener(() => { Toast.Show("adding function to delegate", Toast.LENGTH_LONG); });
            alertDiaolog.onSucessLitener(() => { Toast.Show("on sucess from event listener", Toast.LENGTH_LONG); });
            alertDiaolog.Show("title", "your message", "button 1");
        }

        public void ShowToast()
        {
            Toast.Show("test", Toast.LENGTH_LONG);
        }

        public void ShowIntent()
        {
            TextIntent intent = new TextIntent();
            intent.ShareText("share subject", "share message", "share your score");
        }

        public void ShowCalendar()
        {
            NativeAndroidElements.Calendar cal = new ();
        
            cal.onSelectListener(() => {Toast.Show(  "Выбранная дата " + cal.getSelectedDate().ToString(), Toast.LENGTH_LONG); });
            cal.Show();
        }
    
    
    }
}