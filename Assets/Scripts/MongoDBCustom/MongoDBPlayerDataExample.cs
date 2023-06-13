using MongoDB.Bson;
using MongoDB.Driver;
using NaughtyAttributes;
using UnityEngine;

namespace MongoDBCustom
{
    public class MongoDBPlayerDataExample : MonoBehaviour
    {
        [ResizableTextArea] [SerializeField] private string _jsonExample;

        private void OnEnable()
        {
            _jsonExample = ( GetPlayerDataById());
        }

        private string GetPlayerDataById()
        {
            var filter = Builders<BsonDocument>.Filter.Eq(DBKeys.DeviceID, DeviceInfo.GetDeviceId());
            var document = MongoDBDataHolder.Data.Collection.Find(filter).FirstOrDefault();

            if (document != null)
            {
                return document.ToJson();
            }

            return "";
        }
    }


    // public class JsonFormatter
    // {
    //     public static string FormatJson(string jsonString)
    //     {
    //         try
    //         {
    //             JToken parsedJson = JToken.Parse(jsonString);
    //             string formattedJson = parsedJson.ToString(Formatting.Indented);
    //             return formattedJson;
    //         }
    //         catch (JsonReaderException)
    //         {
    //             // Обработка ошибки парсинга некорректного JSON
    //             return jsonString;
    //         }
    //     }
    // }

    
}