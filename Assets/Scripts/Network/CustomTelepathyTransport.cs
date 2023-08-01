using System;
using Mirror;
using UnityEngine;

namespace Network
{
    public class CustomTelepathyTransport : TelepathyTransport
    {
        [SerializeField] private FirebaseCustom.ConnectionConfig _connectionConfig;
        

        protected override void Awake()
        {
            port =  Convert.ToUInt16(_connectionConfig.Port);

            base.Awake();
        }
    }
}