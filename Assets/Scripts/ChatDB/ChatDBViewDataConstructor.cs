using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
namespace ChatDB
{
    public class ChatDBViewDataConstructor : MonoBehaviour
    {
        [SerializeField] private ChatMessageGameObjectSpawner _chatMessageSpawner;
        [SerializeField] private ChatDBTest _chatDB;

        private ObjectPooler<ChatMessageGameObject> _chatObjPool;
        private HashSet<ChatMessageGameObject> _activeChatObjects = new HashSet<ChatMessageGameObject>();

        private void Awake()
        {
            _chatObjPool = new ObjectPooler<ChatMessageGameObject>(_chatMessageSpawner.CreateChatObject);
            _chatDB.ChatUpdated += OnChatUpdated;
        }

        private void OnDestroy()
        {
            _chatDB.ChatUpdated -= OnChatUpdated;
        }

        private void OnChatUpdated(List<ChatMessage> chatMessages)
        {
            ClearChat();

            FillChat(chatMessages);
        }

        private void FillChat(List<ChatMessage> chatMessages)
        {
            int counter = 0;
            foreach (var chatMessage in chatMessages)
            {
                ChatMessageGameObject chatMessageGameObject = _chatObjPool.Get();
                _activeChatObjects.Add(chatMessageGameObject);
                chatMessageGameObject.gameObject.SetActive(true);
                chatMessageGameObject.gameObject.name = counter++.ToString();
                chatMessageGameObject.transform.SetAsLastSibling();

                chatMessageGameObject.MessageText.text = chatMessage.MessageText;
                DateTime localTime = ConvertUtcToTimeZone(chatMessage.Timestamp);
                chatMessageGameObject.NicknameText.text = chatMessage.PlayerNickname + " " + localTime.ToString("HH:mm");
                ;
            }
        }
        
        public DateTime ConvertUtcToTimeZone(DateTime utcTime)
        {
            TimeSpan localOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
            DateTime localTime = utcTime + localOffset;
            return localTime;
        }

        private void ClearChat()
        {
            foreach (var chatObj in _activeChatObjects)
            {
                chatObj.gameObject.SetActive(false);
            }
        }
    }
}