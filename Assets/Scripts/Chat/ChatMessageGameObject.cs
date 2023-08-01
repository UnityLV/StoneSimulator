using System;
using System.Collections;
using System.Collections.Generic;
using Chat;
using ChatDB;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VRTX.UI;

public class ChatMessageGameObject : MonoBehaviour, IPooleable
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

    public event UnityAction<IPooleable> Deactivation;

    public void OnDisable()
    {
        Deactivation?.Invoke(this);
    }
}
