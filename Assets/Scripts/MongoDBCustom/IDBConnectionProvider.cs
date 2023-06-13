using System;
using UnityEngine.Events;

namespace MongoDBCustom
{
    public interface IDBConnectionProvider
    {
        event UnityAction<MongoDBConnectionData> SuccessConnect;
        event UnityAction<Exception> FailedConnect;
    }
}