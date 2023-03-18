using System;
using System.Collections;
using Mirror;
using Network.Interfaces;
using UnityEngine;

namespace Network
{
    public class NetworkCallbackController : NetworkBehaviour, INetworkCallbacks
    {
        private static NetworkIdentity _networkIdentity;
        private int _clickCount;
        private readonly float _serverTimer = 1f;

        public override void OnStartClient()
        {
            base.OnStartClient();
            _networkIdentity = GetComponent<NetworkIdentity>();
            DontDestroyOnLoad(this);
        }

        public override void OnStopClient()
        {
            Destroy(gameObject);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            DontDestroyOnLoad(this);
            StartCoroutine(IServerClickProcess());
        }
        
        public event Action<int> OnStoneClickNetwork;
        public void AddStoneClickOnServer()
        {
            CmdAddClickOnServer(_networkIdentity.connectionToClient);
        }

        public float GetServerCallbackTimer()
        {
            return _serverTimer;
        }

        [Command(requiresAuthority =  false)]
        private void CmdAddClickOnServer(NetworkConnectionToClient connectionToClient)
        {
            _clickCount += 1;
            Debug.Log("Somebody click on stone");
        }

        [ClientRpc]
        private void InvokeStoneClickOnClient(int count)
        {
            InvokeStoneClick(count);
        }

        private void InvokeStoneClick(int count)
        {
            OnStoneClickNetwork?.Invoke(count);
        }


        private IEnumerator IServerClickProcess()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(_serverTimer);
                InvokeStoneClickOnClient(_clickCount);
                _clickCount = 0;
            }
        }
        
    }
}