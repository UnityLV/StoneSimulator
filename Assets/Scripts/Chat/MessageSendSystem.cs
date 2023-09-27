using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace Chat
{
    public class MessageSendSystem : NetworkBehaviour
    {
        [SerializeField] private UnityEvent _onReceiveMessage;
        private void Update()
        {
            if (Input.GetKeyDown( KeyCode.S))
            {
                SendMessage();
            }
        }

        [Command(requiresAuthority = false)]
        void CmdSend()
        {
            RpcReceive();
        }

        [ClientRpc]
        void RpcReceive()
        {
            Debug.Log("Receive Message in chat");
            _onReceiveMessage.Invoke();
        }
        public void SendMessage()
        {
            Debug.Log("Send Message in chat");
            CmdSend();
        }

    }

    [Serializable]
    public struct ChatMsg
    {
        public string Nickname;
        public string Message;

        public ChatMsg(string nickname, string message)
        {
            Nickname = nickname;
            Message = message;
        }
    }
}