using System;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace MongoDBCustom
{
    public interface IDBConnector
    {
        Task TryConnect();
    }
}