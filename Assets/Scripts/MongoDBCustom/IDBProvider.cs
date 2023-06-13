using System;
using UnityEngine.Events;

namespace MongoDBCustom
{
    public interface IDBProvider
    {
        event UnityAction<MongoDBConnectionData> SuccessConnect;
        event UnityAction<Exception> FailedConnect;
    }
}