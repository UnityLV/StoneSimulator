using System;
using Mirror;
using UnityEngine;

namespace DatabaseNetwork
{
    public class TestBehavior: NetworkBehaviour
    {
        void Update()
        {
            if (Input.GetKey(KeyCode.X))
            {
                SendLog();
                CmdTestLog();
            }
        }

    
        void SendLog()
        {
            Debug.Log("Log Sended!");
        }
        
        [Command]
        void CmdTestLog()
        {
            Debug.Log("Player press X");
        }
    }
}