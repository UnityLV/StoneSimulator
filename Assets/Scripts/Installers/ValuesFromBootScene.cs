using MongoDB.Bson;
using MongoDBCustom;
using Network;

namespace Installers
{
    public class ValuesFromBootScene
    {
        public static IDBCommands IdbCommands { get; set; }
        public static CustomNetworkManager CustomNetworkManager { get; set; }
        public static IMongoConnection MongoConnection { get; set; }
        public static BsonDocument PlayerData { get; set; }
        
        public static int HealthPointPerLevel { get; set; }
    }
}