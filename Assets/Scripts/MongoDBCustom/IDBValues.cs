using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDBCustom
{
    public interface IDBValues
    {
        Task UpdatePlayerName(string result);
        Task SetMeAsRefferalTo(string refferalId);
        Task AddPlayerClickToGiveReferrer(int clicksToAdd);
        Task<List<BsonDocument>> PlayersRating();
        Task AddAllPlayerClicks(int add);
        Task<BsonDocument> GetPlayerDataAsync();
    }
}