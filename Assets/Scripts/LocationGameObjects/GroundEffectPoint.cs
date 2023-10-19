using UnityEngine;
using UnityEngine.Serialization;

namespace LocationGameObjects
{
    public class GroundEffectPoint : MonoBehaviour
    {
        [SerializeField] private Transform _point;

        public Transform GetEffectPoint()
        {
            return _point;
        }
    }
}