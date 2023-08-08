using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace ChatDB
{
    public class ChatDBModel : MonoBehaviour
    {
        [SerializeField] private ChatDB _chatDB;

        [SerializeField] private TMP_InputField _inputField;

        [SerializeField] Scrollbar _scrollbar;


        public event Action<List<ChatMessage>> ChatUpdated;

        private float _updateTimer = 3f;

        private bool _isAvalableForSending = true;
        private bool _isAvalableForUpdating = true;

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
            _isAvalableForSending = false;
            _isAvalableForUpdating = false;

            await _chatDB.InsertMessage(message);

            _isAvalableForUpdating = true;
            await UpdateChat();

            _isAvalableForSending = true;
        }

        public void EnableChatUpdating()
        {
            _isAvalableForUpdating = true;
        }

        public async Task UpdateChat()
        {
            if (_isAvalableForUpdating == false)
            {
                return;
            }
            _isAvalableForUpdating = false;
            List<ChatMessage> chatMessages = await _chatDB.GetLastChatMessagesAsync(20);

            ChatUpdated?.Invoke(chatMessages);
            _isAvalableForUpdating = true;
        }
    }
}