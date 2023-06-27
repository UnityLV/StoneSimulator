using System.Collections.Generic;
using MainMenuUI;
using MongoDB.Bson;

namespace PlayerData.Interfaces
{
    public interface INicknameDataService
    {
        public void SetNickname(string nickname);
        public string GetNickname();
    }

    public interface ISlavesDataService
    {
        void SetSlaves(List<BsonDocument> slaves);
        SlavesData GetSlaves();
    }
}