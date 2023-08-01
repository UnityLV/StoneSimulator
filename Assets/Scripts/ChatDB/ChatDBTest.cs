using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
namespace ChatDB
{
    public class ChatDBTest : MonoBehaviour
    {
        [SerializeField] private ChatDB _chatDB;

        [SerializeField] private string _message;

        public event Action<List<ChatMessage>> ChatUpdated;

        [Button()]
        public async void Send()
        {
            if (string.IsNullOrEmpty(_message))
                return;

            await _chatDB.InsertMessage(_message);
        }

        [Button()]
        public async void GetChat()
        {
            List<ChatMessage> chatMessages = await _chatDB.GetLastChatMessagesAsync(20);

            ChatUpdated?.Invoke(chatMessages);
            
            foreach (var message in chatMessages)
            {
                Debug.Log(message);
            }
        }
    }
}