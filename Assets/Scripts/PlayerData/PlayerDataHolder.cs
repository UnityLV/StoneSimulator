using PlayerData.Interfaces;
using SaveSystem;
using UnityEngine;

namespace PlayerData
{
    public class PlayerDataHolder:INicknameDataService, IClickDataService
    {
        private static NicknameData nicknameData;
        private BinarySaveSystem  nicknameSaveSystem;
        private const string NICKNAME_DATA_PATCH = "NicknameData";

        private static ClickData _clickData;
        private BinarySaveSystem  _clickSaveSystem;
        private const string CLICK_DATA_PATCH = "ClickData";

        public PlayerDataHolder()
        {
            LoadNicknameData();
            LoadClickData();
        }

        private void LoadNicknameData()
        {
            nicknameSaveSystem ??= new BinarySaveSystem(NICKNAME_DATA_PATCH);
            nicknameData =(NicknameData) nicknameSaveSystem.Load();
            if (nicknameData == null)
            {
                nicknameData = new NicknameData();
                SaveNicknameData();
            }
        }

        private void SaveNicknameData()
        {
            
            nicknameSaveSystem.Save(nicknameData);
        } 
        
        private void LoadClickData()
        {
            _clickSaveSystem ??= new BinarySaveSystem(CLICK_DATA_PATCH);
            _clickData =(ClickData) _clickSaveSystem.Load();
            if (_clickData == null)
            {
                _clickData = new ClickData();
                SaveClickData();
            }
        }

        private void SaveClickData()
        {
            
            _clickSaveSystem.Save(_clickData);
        }

        public void SetNickname(string nickname)
        {
            string result = nickname.Substring(0, Mathf.Min(nickname.Length,15));
            nicknameData.CurrentNickname = result;
            SaveNicknameData();
        }

        public string GetNickname()
        {
            return nicknameData.CurrentNickname;
        }

        public int GetClickCount()
        {
            return _clickData.ClickCount;
        }

        public void SetClickCount(int value)
        {
            _clickData.ClickCount = value;
            SaveClickData();
        }

        public void AddClick()
        {
            _clickData.ClickCount +=1;
            SaveClickData();
        }
    }
}