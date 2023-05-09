using Mirror;
using UnityEngine;

public class PlayerBehavior : NetworkBehaviour
{
    public override void OnStartClient()
    {
        DontDestroyOnLoad(this);
    }
}