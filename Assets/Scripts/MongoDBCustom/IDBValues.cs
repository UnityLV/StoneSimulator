using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDBCustom
{
    public interface IDBValues
    {
        Task InsertPlayerDataAsync(BsonDocument playerData);
        Task RemoveMeFromReferral();
        Task UpdatePlayerName(string result);
        Task AddPlayerClickToGiveReferrer(int clicksToAdd);
        Task AddAllPlayerClicks(int add);
        Task AddMeAsReferralsTo(string newReferrer);
        Task<List<BsonDocument>> CollectClicksToGiveReferrer(List<string> slavesId);
        Task<List<BsonDocument>> PlayersRating();
        Task<BsonDocument> GetPlayerDataAsync();
        Task<List<BsonDocument>> GetPlayersDataById(params string[] id);
    }
}