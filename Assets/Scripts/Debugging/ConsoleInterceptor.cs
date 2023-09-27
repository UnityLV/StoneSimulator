using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Debugging
{
    public class ConsoleInterceptor : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private Queue<string> _toShow = new Queue<string>();

        void Awake()
        {
            Application.logMessageReceived += HandleLog;
            ShowMessage();
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            _toShow.Enqueue(logString);
        }

        private async void ShowMessage()
        {
            if (_toShow.TryDequeue(out string result))
            {
                _text.text = result;
            }
            await Task.Delay(540);
            ShowMessage();
        }
    }
}