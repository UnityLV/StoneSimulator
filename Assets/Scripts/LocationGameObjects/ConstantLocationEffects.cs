using UnityEngine;

namespace LocationGameObjects
{
    public interface IConstantLocationEffects
    {
        void ShowConstantEffect(int location, int stoneLvl);
        void SetConstantEffectPoint(Transform point);
    }

    public class ConstantLocationEffects : IConstantLocationEffects
    {
        private GameObject _currentConstantEffect;
        private Transform _constantEffectPoint;
        private IGetLocationConstantEffectService _locationConstantEffect;

        public ConstantLocationEffects(IGetLocationConstantEffectService locationConstantEffect)
        {
            _locationConstantEffect = locationConstantEffect;
        }

        public void ShowConstantEffect(int location, int stoneLvl)
        {
            if (_currentConstantEffect != null)
            {
                HideConstantEffect();
            }

            CreateEffect(location, stoneLvl);

            //Debug.Log($"Show Constant Effect: {location} {stoneLvl}");
        }

        public void HideConstantEffect()
        {
            DeleteEffect();
            _currentConstantEffect = null;
        }

        private void CreateEffect(int location, int stoneLvl)
        {
            var effect = _locationConstantEffect.GetConstantEffect(location, stoneLvl);
            _currentConstantEffect = Object.Instantiate(effect, GetSpawnPoint(), Quaternion.identity);
        }

        private Vector3 GetSpawnPoint()
        {
            return _constantEffectPoint != null ? _constantEffectPoint.position : Vector3.down;
        }

        private void DeleteEffect()
        {
            Object.Destroy(_currentConstantEffect);
        }

        public void SetConstantEffectPoint(Transform point)
        {
            _constantEffectPoint = point;
        }
    }
}