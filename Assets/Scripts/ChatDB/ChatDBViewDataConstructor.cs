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
        private List<ChatMessageGameObject> _activeChatObjects = new List<ChatMessageGameObject>();

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
            foreach (var chatMessage in chatMessages)
            {
                ChatMessageGameObject chatMessageGameObject = _chatObjPool.Get();
                chatMessageGameObject.gameObject.SetActive(true);

                chatMessageGameObject.MessageText.text = chatMessage.MessageText;
                chatMessageGameObject.NicknameText.text = chatMessage.PlayerNickname;
            }
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