using System;
using Mirror;
using UnityEngine;

namespace Network
{
    public class CustomTelepathyTransport : TelepathyTransport
    {
        [SerializeField] private FirebaseCustom.ConnectionConfig _connectionConfig;
        
        [SerializeField] private bool  _isUseLocalHost;

        protected override void Awake()
        {
            port = _isUseLocalHost ? (ushort)34120 : Convert.ToUInt16(_connectionConfig.Port);

            base.Awake();
        }
    }
}