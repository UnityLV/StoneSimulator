using UnityEngine;

namespace LocationGameObjects
{
    [CreateAssetMenu]
    public class AbilityClickEffectDataHolder : ScriptableObject, IGetAbilityEffectService
    {
        [SerializeField] private GameObject[] _effects;
        [SerializeField] private GameObject[] _topEffects;

        public GameObject GetGroundEffect(int clicks)
        {
            if (clicks <= 150)
            {
                return _effects[0];
            }

            if (clicks <= 300)
            {
                return _effects[1];
            }

            return _effects[2];
        }

        public GameObject GetTopEffect(int clicks)
        {
            if (clicks <= 150)
            {
                return _topEffects[0];
            }

            if (clicks <= 300)
            {
                return _topEffects[1];
            }

            return _topEffects[2];
        }
    }

    public interface IGetAbilityEffectService
    {
        public GameObject GetGroundEffect(int clicks);
        public GameObject GetTopEffect(int clicks);
    }
}