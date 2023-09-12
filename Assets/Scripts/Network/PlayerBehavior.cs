using System;
using GameScene;
using Mirror;
using NaughtyAttributes;
using Network;
using UnityEngine;


public class PlayerBehavior : NetworkBehaviour
{
    public override void OnStartClient()
    {
        DontDestroyOnLoad(this);

        if (isLocalPlayer)
        {
            AbilityClicks.PlayerBehavior = this;
        }
    }

    #region PlayerDamage

    [SyncVar]
    public int playerDamageOnServer = 1;
 
    public void SetDamage(int damage)
    {
        if (isLocalPlayer == false)
            return;
        
        playerDamageOnServer = 1;
        playerDamageOnServer = damage;
        Debug.Log(" set damage to " + playerDamageOnServer);
    }

    #endregion
}