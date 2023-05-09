using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using PlayerData.Interfaces;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Chat
{
    public class ChatUI : NetworkBehaviour
    {
        [Header("Scale properties"), SerializeField]
        private RectTransform _mainCanvas;

        [SerializeField]
        private Vector2 _defaultResolution;

        [Header("UI Elements"), SerializeField]
        private GameObject chatObjectPrefab;

        [SerializeField]
        private Transform _chatParent;

        private List<ChatObj> _messages;

        [SerializeField]
        Scrollbar scrollbar;

        [SerializeField]
        TMP_InputField chatMessage;

        [SerializeField]
        Button sendButton;

        [Header("Pined message"), SerializeField]
        private ChatObj _pinnedObj;

        [SerializeField]
        private RectTransform _pinnedRect;

        [SerializeField]
        private RectTransform _viewRect;

        private Coroutine _realisingPin;

        private const float DEFAULT_TOP = 45;
        private const float SPACING_TOP = 32;

        // This is only set on client to the name of the local player
        internal static string localPlayerName;

        // Server-only cross-reference of connections to player names
        private static readonly Dictionary<NetworkConnectionToClient, string> _connNames =
            new Dictionary<NetworkConnectionToClient, string>();

        private const float SAVE_TIME = 300;

        private NetworkIdentity _networkIdentity;

        private static ChatData _chatData;
        private BinarySaveSystem _chatSaveSystem;
        private const string CHAT_DATA_PATCH = "ChatData";

        private static PinnedMsgData _pinnedMsgData;
        private BinarySaveSystem _pinnedSaveSystem;
        private const string PINNED_DATA_PATCH = "PinnedData";

        #region Dependency

        private INicknameDataService _nicknameDataService;

        [Inject]
        private void Construct(INicknameDataService nicknameDataService)
        {
            _nicknameDataService = nicknameDataService;
        }

        #endregion

        public override void OnStartServer()
        {
            _connNames.Clear();
            LoadChatData();
            LoadPinData();
            if (_pinnedMsgData.ReleaseDateTime > DateTime.Now)
            {
                if (_realisingPin != null) StopCoroutine(_realisingPin);
                _realisingPin = StartCoroutine(IPinReleasing());
            }

            StartCoroutine(IServerChatSaver());
        }

        public override void OnStartClient()
        {
            _networkIdentity = GetComponent<NetworkIdentity>();
            _messages = new List<ChatObj>();
            CmdLoadMsg(_networkIdentity.connectionToClient);
            CmdLoadPinMsg(_networkIdentity.connectionToClient);
        }

        public override void OnStopServer()
        {
            if (isServer) SaveChatData();
            base.OnStopServer();
        }

        private void Update()
        {
            if (!isClient) return;
            if (Input.GetKeyDown(KeyCode.P))
                CmdPinMessage("Tester", "its a test msg", _networkIdentity.connectionToClient);
        }

        [Command(requiresAuthority = false)]
        private void CmdLoadMsg(NetworkConnectionToClient sender)
        {
            for (int i = 0; i < _chatData.ChatHistory.Count; i++)
            {
                TargetRpcReceive(sender, _chatData.ChatHistory[i].Nickname, _chatData.ChatHistory[i].Message);
            }
        }

        [Command(requiresAuthority = false)]
        private void CmdLoadPinMsg(NetworkConnectionToClient sender)
        {
            if (_pinnedMsgData.ReleaseDateTime > DateTime.Now)
                TargetRPCPinMsg(sender, _pinnedMsgData.PinnedMessage.Nickname, _pinnedMsgData.PinnedMessage.Message);
            else TargetRPCUnpinMessageObject(sender);
        }

        [Command(requiresAuthority = false)]
        void CmdSend(string nickname, string message, NetworkConnectionToClient sender = null)
        {
            if (!_connNames.ContainsKey(sender))
                _connNames.Add(sender, nickname);

            if (string.IsNullOrWhiteSpace(message)) return;
            _chatData.ChatHistory.Add(new ChatMsg(nickname, message));
            RpcReceive(_connNames[sender], message.Trim());
            Debug.Log($"Message in chat {sender}, {nickname}: {message}");
        }

        [ClientRpc]
        void RpcReceive(string playerName, string message)
        {
            AppendMessage(playerName, message);
        }

        [TargetRpc]
        void TargetRpcReceive(NetworkConnectionToClient target, string playerName, string message)
        {
            AppendMessage(playerName, message);
        }

        void AppendMessage(string playerName, string message)
        {
            StartCoroutine(AppendAndScroll(playerName, message));
        }

        IEnumerator AppendAndScroll(string nickname, string message)
        {
            var messageObj = Instantiate(chatObjectPrefab).GetComponent<ChatObj>();
            messageObj.MessageText.text = message;
            messageObj.NicknameText.text = nickname;
            messageObj.SetChatUI(this);
            messageObj.transform.SetParent(_chatParent);
            messageObj.transform.localScale = Vector3.one;

            _messages.Add(messageObj);

            // it takes 2 frames for the UI to update ?!?!
            yield return null;
            yield return null;

            // slam the scrollbar down
            scrollbar.value = 0;
        }

        // Called by UI element MessageField.OnValueChanged
        public void ToggleButton(string input)
        {
            sendButton.interactable = !string.IsNullOrWhiteSpace(input);
        }

        // Called by UI element MessageField.OnEndEdit
        public void OnEndEdit(string input)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) ||
                Input.GetButtonDown("Submit"))
                SendMessage();
        }

        // Called by OnEndEdit above and UI element SendButton.OnClick
        public void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(chatMessage.text))
            {
                CmdSend(_nicknameDataService.GetNickname(), chatMessage.text.Trim());
                chatMessage.text = string.Empty;
                chatMessage.ActivateInputField();
            }
        }

        private void LoadChatData()
        {
            _chatSaveSystem = new BinarySaveSystem(CHAT_DATA_PATCH);
            _chatData = (ChatData) _chatSaveSystem.Load();
            if (_chatData == null)
            {
                _chatData = new ChatData();
                SaveChatData();
            }
        }

        private void SaveChatData()
        {
            _chatSaveSystem.Save(_chatData);
        }

        private void LoadPinData()
        {
            _pinnedSaveSystem = new BinarySaveSystem(PINNED_DATA_PATCH);
            _pinnedMsgData = (PinnedMsgData) _pinnedSaveSystem.Load();
            if (_pinnedMsgData == null)
            {
                _pinnedMsgData = new PinnedMsgData();
                SavePinnedData();
            }
        }

        private void SavePinnedData()
        {
            _pinnedSaveSystem.Save(_pinnedMsgData);
        }

        private IEnumerator IServerChatSaver()
        {
            while (true)
            {
                yield return new WaitForSeconds(SAVE_TIME);
                SaveChatData();
                Debug.Log("Chat saved!");
            }
        }

        [Command(requiresAuthority = false)]
        private void CmdPinMessage(string nickname, string message, NetworkConnectionToClient client = null)
        {
            _pinnedMsgData.PinnedMessage = new ChatMsg(nickname, message);
            _pinnedMsgData.ReleaseDateTime = DateTime.Now.AddDays(1);
            SavePinnedData();
            if (_realisingPin != null) StopCoroutine(_realisingPin);
            _realisingPin = StartCoroutine(IPinReleasing());
            ClientRPCPinMsg(nickname, message);
        }

        public void PinMessage(string nickname, string message)
        {
            CmdPinMessage(nickname,message,_networkIdentity.connectionToClient);
        }
        
        
        [ClientRpc]
        private void ClientRPCPinMsg(string nickname, string message)
        {
            StartCoroutine(PinMessageObject(new ChatMsg(nickname, message)));
        }

        [TargetRpc]
        private void TargetRPCPinMsg(NetworkConnectionToClient target, string nickname, string message)
        {
            StartCoroutine(PinMessageObject(new ChatMsg(nickname, message)));
        }


        [Command(requiresAuthority = false)]
        private void CmdUnpinMessage()
        {
            _pinnedMsgData.ReleaseDateTime = DateTime.MinValue;
            SavePinnedData();
            if (_realisingPin != null) StopCoroutine(_realisingPin);
            _realisingPin = null;
            ClientRPCUnpinMessageObject();
        }

        [ClientRpc]
        private void ClientRPCUnpinMessageObject()
        {
            _pinnedObj.gameObject.SetActive(false);
            _viewRect.offsetMax = new Vector2(_viewRect.offsetMax.x, -DEFAULT_TOP);
        }

        [TargetRpc]
        private void TargetRPCUnpinMessageObject(NetworkConnectionToClient target)
        {
            _pinnedObj.gameObject.SetActive(false);
            _viewRect.offsetMax = new Vector2(_viewRect.offsetMax.x, -DEFAULT_TOP);
        }

        private IEnumerator PinMessageObject(ChatMsg msg)
        {
            _pinnedObj.gameObject.SetActive(true);
            _pinnedObj.NicknameText.text = msg.Nickname;
            _pinnedObj.MessageText.text = msg.Message;
            yield return null;
            yield return null;
            yield return null;
            _viewRect.offsetMax = new Vector2(_viewRect.offsetMax.x, -(_pinnedRect.rect.height + SPACING_TOP));
        }

        private IEnumerator IPinReleasing()
        {
            while (_pinnedMsgData.ReleaseDateTime > DateTime.Now)
            {
                yield return new WaitForSeconds(1);
            }
            _pinnedMsgData.ReleaseDateTime = DateTime.MinValue;
            SavePinnedData();
            if (_realisingPin != null) StopCoroutine(_realisingPin);
            _realisingPin = null;
            ClientRPCUnpinMessageObject();
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