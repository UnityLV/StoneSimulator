using UnityEngine;
namespace ChatDB
{
    public class ChatMessageGameObjectSpawner : MonoBehaviour
    {
        
        [SerializeField] private Transform _chatParent;
        [SerializeField] private ChatMessageGameObject _chatObjectPrefab;
        
        public ChatMessageGameObject CreateChatObject()
        {
            ChatMessageGameObject chatMessageGameObject = Instantiate(_chatObjectPrefab).GetComponent<ChatMessageGameObject>();
            Transform chatObjTransform = chatMessageGameObject.transform;

            chatObjTransform.SetParent(_chatParent);
            chatObjTransform.localScale = Vector3.one;

            return chatMessageGameObject;
        }

    }
}