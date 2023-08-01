using UnityEngine.Events;
namespace ChatDB
{
    public interface IPooleable
    {
        event UnityAction<IPooleable> Deactivation;
    }
}