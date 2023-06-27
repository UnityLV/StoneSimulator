using MongoDBCustom;
using Network;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ValuesFromBootScene
    {
        public static IDBValues DBValues { get; set; }
        public static CustomNetworkManager CustomNetworkManager { get; set; }
        public static IMongoConnection MongoConnection { get; set; }
    }
}