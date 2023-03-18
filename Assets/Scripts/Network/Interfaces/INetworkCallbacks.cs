using System;

namespace Network.Interfaces
{
    public interface INetworkCallbacks
    {
        public event Action<int> OnStoneClickNetwork;
        public void AddStoneClickOnServer();

        public float GetServerCallbackTimer();
    }
}