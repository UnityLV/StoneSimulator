using System.Threading.Tasks;
namespace MongoDBCustom
{
    public interface IDBAllClickSaver
    {
        Task Save(int amount  = 0);
    }
}