using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ChatDB
{
    public class ChatDBModel : MonoBehaviour
    {
        [SerializeField] private ChatDB _chatDB;

        [SerializeField] private TMP_InputField _inputField;

        [SerializeField] Scrollbar _scrollbar;

        public event UnityAction<List<ChatMessage>> ChatUpdated;

        private float _updateTimer = 4f;
        private bool _isAvalableForUpdating = true;
        private bool _isInWork;

        private void Awake()
        {
#if !UNITY_SERVER
            InvokeRepeating(nameof(UpdateChat), _updateTimer, _updateTimer);
#endif
        }

        public async void Send()
        {
            string message = _inputField.text;
            if (string.IsNullOrEmpty(message))
                return;

            _inputField.text = "";
            await ProcessSending(message);
            _scrollbar.value = 0;
        }

        private async Task ProcessSending(string message)
        {
            _isAvalableForUpdating = false;

            await _chatDB.InsertMessage(message);

            _isAvalableForUpdating = true;
            await UpdateChat();
        }

        public void EnableChatUpdating()
        {
            _isAvalableForUpdating = true;
        }

        public async Task UpdateChat()
        {
            if (_isAvalableForUpdating == false || _isInWork)
            {
                return;
            }

            _isAvalableForUpdating = false;
            _isInWork = true;
            Debug.Log("Update Chat History");
            await UpdateChatJob();
            _isInWork = false;
        }

        private async Task UpdateChatJob()
        {
            int messagesToLoad = 20;
            List<ChatMessage> chatMessages = await _chatDB.GetLastChatMessagesAsync(messagesToLoad);

            ChatUpdated?.Invoke(chatMessages);
        }
    }
}