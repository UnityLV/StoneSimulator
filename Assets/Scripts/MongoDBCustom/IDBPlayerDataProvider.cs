using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDBCustom
{
    public interface IDBPlayerDataProvider
    {
        Task<BsonDocument> GetPlayerDataById();
    }
}