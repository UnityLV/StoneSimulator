using System;
using System.Collections.Generic;
using Chat;
using UnityEngine;

[Serializable]
public class StoneData : ISaveble
{
    public int CurrentLocation = 0;
    public int CurrentStone = 0;
    public int CurrentHealth = 0;
}

[Serializable]
public class NicknameData : ISaveble
{
    public string CurrentNickname = "Newbie";
}

[Serializable]
public class ChatData : ISaveble
{
    public List<ChatMsg> ChatHistory = new List<ChatMsg>();
}

[Serializable]
public class PinnedMsgData : ISaveble
{
    public ChatMsg PinnedMessage = new ChatMsg();
    public DateTime ReleaseDateTime = new DateTime();
}

[Serializable]
public class HealthData : ISaveble
{
    public List<IntArray> Healths = new ();
}

[Serializable]
public class ClickData : ISaveble
{
    public int ClickCount = 0;
}

[Serializable]
public class IntArray
{
    public List<int> Array = new();
}

public interface ISaveble
{
}