using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace ChatDB
{
    public class ChatDBViewDataConstructor : MonoBehaviour
    {
        [SerializeField] private ChatMessageGameObjectSpawner _chatMessageSpawner;
        [SerializeField] private ChatDBModel _chatDB;
        [SerializeField] private UnityEvent _chatFielded;

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

        private List<ChatMessage> _lastMessages;

        private bool IsMessagesEqual(List<ChatMessage> chatMessages)
        {
            if (_lastMessages == null)
            {
                return false;
            }

            if (_lastMessages.Count != chatMessages.Count)
            {
                return false;
            }

            for (int i = 0; i < chatMessages.Count; i++)
            {
                if (chatMessages[i].Timestamp != _lastMessages[i].Timestamp)
                {
                    return false;
                }
            }

            return true;
        }

        private void OnChatUpdated(List<ChatMessage> chatMessages)
        {
            if (IsMessagesEqual(chatMessages))
            {
                return;
            }
            
            ClearChat();

            FillChat(chatMessages);

            _chatFielded.Invoke();
        }

        private void FillChat(List<ChatMessage> chatMessages)
        {
            int counter = 0;
            foreach (var chatMessage in chatMessages)
            {
                ChatMessageGameObject chatMessageGameObject = _chatObjPool.Get();
                _activeChatObjects.Add(chatMessageGameObject);
                SetDataInChatMessageUILine(chatMessageGameObject, counter, chatMessage);
                counter++;
            }
        }

        private void SetDataInChatMessageUILine(ChatMessageGameObject chatMessageGameObject, int counter,
            ChatMessage chatMessage)
        {
            chatMessageGameObject.gameObject.SetActive(true);
            chatMessageGameObject.gameObject.name = counter.ToString();
            chatMessageGameObject.transform.SetAsLastSibling();
            chatMessageGameObject.MessageText.text = chatMessage.MessageText;

            SetNickName(chatMessage, chatMessageGameObject);
        }

        private void SetNickName(ChatMessage chatMessage, ChatMessageGameObject chatMessageGameObject)
        {
            DateTime localTime = ConvertUtcToTimeZone(chatMessage.Timestamp);
            chatMessageGameObject.NicknameText.text = chatMessage.PlayerNickname + " " + localTime.ToString("HH:mm");
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