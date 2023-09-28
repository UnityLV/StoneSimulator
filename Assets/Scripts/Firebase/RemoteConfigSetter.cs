using Installers;
using PlayerData;

namespace FirebaseCustom
{
    public class RemoteConfigSetter
    {
        private readonly ConnectionConfig _unityServerConnectionConfig;
        private readonly ConnectionConfig _dbConnectionConfig;
        private readonly PlayerConfig _playerConfig;
        private readonly RankData _rankData;

        public static PlayerConfig PlayerConfig { get; private set; }

        public RemoteConfigSetter(ConnectionConfig unityServerConnectionConfig, ConnectionConfig dbConnectionConfig, PlayerConfig playerConfig,RankData rankData)
        {
            _unityServerConnectionConfig = unityServerConnectionConfig;
            _dbConnectionConfig = dbConnectionConfig;
            _playerConfig = playerConfig;
            _rankData = rankData;
        }

        public void SetConfig(Config config)
        {
            PlayerConfig = _playerConfig;
            
            _unityServerConnectionConfig.Ip = config.UnityServerIp;
            _unityServerConnectionConfig.Port = config.UnityServerPort;
            _dbConnectionConfig.Ip = config.DBServerIp;
            _dbConnectionConfig.Port = config.DBServerPort;
            _playerConfig.ClicksToRedeemed = config.ClicksToRedeemed;
            _playerConfig.PercentToAddToReferrer = config.PercentToAddToReferrer;
            _playerConfig.EarnedFromEachReferral = config.EarnedFromEachReferral;
            _playerConfig.ClicksFromAD = config.ClicksFromAD;
            _rankData.FromJson(config.RanksJson);
        }
    }
}