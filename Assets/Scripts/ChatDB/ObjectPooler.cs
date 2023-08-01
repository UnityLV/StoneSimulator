using System;
using System.Collections.Generic;
using UnityEngine;
namespace ChatDB
{
    public class ObjectPooler<T> where T : MonoBehaviour, IPooleable
    {

        private readonly Stack<IPooleable> _pool = new();
        private readonly Func<T> _spawn;

        public ObjectPooler(Func<T> spawn)
        {
            _spawn = spawn;
        }

        public T Get()
        {
            if (_pool.TryPop(out IPooleable pooleable))
            {
                pooleable.Deactivation += OnDeactivation;
                return pooleable as T;
            }

            IPooleable poolable = _spawn();
            poolable.Deactivation += OnDeactivation;

            return poolable as T;
        }

        private void OnDeactivation(IPooleable pooleable)
        {
            pooleable.Deactivation -= OnDeactivation;

            _pool.Push(pooleable);
        }
    }
}