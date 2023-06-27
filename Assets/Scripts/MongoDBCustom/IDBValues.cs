using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDBCustom
{
    public interface IDBValues
    {
        Task InsertPlayerDataAsync(BsonDocument playerData);
        
        Task UpdatePlayerName(string result);
        Task AddPlayerClickToGiveReferrer(int clicksToAdd);
        Task AddAllPlayerClicks(int add);
        Task SetMeAsRefferalTo(string refferalId);
        Task<List<BsonDocument>> CollectClicksToGiveReferrer(List<string> slavesId);
        Task<List<BsonDocument>> PlayersRating();
        Task<BsonDocument> GetPlayerDataAsync();
        Task<List<BsonDocument>> GetPlayersDataById(IEnumerable<string> id);
        
    }
}