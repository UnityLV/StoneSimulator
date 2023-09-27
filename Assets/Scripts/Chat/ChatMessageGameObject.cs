using ChatDB;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ChatMessageGameObject : MonoBehaviour, IPooleable
{
    public TextMeshProUGUI NicknameText;
    public TextMeshProUGUI MessageText;
 
    public event UnityAction<IPooleable> Deactivation;

    public void OnDisable()
    {
        Deactivation?.Invoke(this);
    }
}
