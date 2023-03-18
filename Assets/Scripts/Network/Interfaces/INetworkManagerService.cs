using Network.Enum;

namespace Network.Interfaces
{
    public interface INetworkManagerService
    {
        public ConnectionType GetConnectionType();
    }
}