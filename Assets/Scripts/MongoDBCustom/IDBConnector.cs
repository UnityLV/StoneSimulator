using System.Threading.Tasks;

namespace MongoDBCustom
{
    public interface IDBConnector
    {
        Task TryConnect();
    }
}