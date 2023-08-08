using System;
using Mirror;
using NaughtyAttributes;
using UnityEngine;

public class PlayerBehavior : NetworkBehaviour
{
    public override void OnStartClient()
    {
        DontDestroyOnLoad(this);
    }
    
//Регион дерьма
#region PlayerDamage 

    [SyncVar]
    public int playerDamageOnServer = 1;

    private void Update()
    {
        if (isLocalPlayer == false)
            return;

        if (Input.GetKeyDown(KeyCode.D))
        {
            playerDamageOnServer = 20;
            Debug.Log(" set damage to " + playerDamageOnServer);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            playerDamageOnServer = 1;
            Debug.Log(" set damage to " + playerDamageOnServer);
        }
    }
    
#endregion
}