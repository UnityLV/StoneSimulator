using Mirror;
using UnityEngine;

public class PlayerBehavior : NetworkBehaviour
{
    private static NetworkIdentity _networkIdentity;
    
    private void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.Space)) CmdLog(_networkIdentity.connectionToClient);
    }

    [Command]
    public void CmdLog(NetworkConnectionToClient target)
    {
        Debug.Log($"Долбаеб на {target.owned} нажал Space");
        RpcSendLog();
        RpcSendLogAll();
    }

    [TargetRpc]
    public void RpcSendLog()
    {
        Debug.Log("Ты долбаеб?");
    }

    [ClientRpc]
    private void RpcSendLogAll()
    {
        Debug.Log("Эй, там долбаеб нажала на Space, прикиньте");
    }

    public override void OnStartClient()
    {
        _networkIdentity = GetComponent<NetworkIdentity>();
        DontDestroyOnLoad(this);
    }
}