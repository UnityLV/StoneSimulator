using System.Collections;
using System.Collections.Generic;
using Chat;
using TMPro;
using UnityEngine;
using VRTX.UI;

public class ChatObj : MonoBehaviour
{
    public TextMeshProUGUI NicknameText;
    public TextMeshProUGUI MessageText;

    private ChatUI _chatUI;

    public void SetChatUI(ChatUI chatUI)
    {
        _chatUI = chatUI;
    }

    public void OnMsgClick()
    {
        if (_chatUI!=null) _chatUI.PinMessage(NicknameText.text, MessageText.text);
    }
}
