using PlayerData.Interfaces;
using UnityEngine;

namespace PlayerData
{
    public class PlayerNicknameData : BasePlayerData<NicknameData>, INicknameDataService
    {
        private const string NICKNAME_DATA_PATCH = "NicknameData";

        public PlayerNicknameData() : base(NICKNAME_DATA_PATCH)
        {
            Load();
        }

        public void SetNickname(string nickname)
        {
            string result = nickname.Substring(0, Mathf.Min(nickname.Length, 15));
            Data.CurrentNickname = result;
            Save();
        }

        public string GetNickname()
        {
            return Data.CurrentNickname;
        }
    }
}