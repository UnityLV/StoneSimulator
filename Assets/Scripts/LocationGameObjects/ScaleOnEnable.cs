using DG.Tweening;
using UnityEngine;

namespace LocationGameObjects
{
    public class ScaleOnEnable : MonoBehaviour
    {
        [SerializeField] private float _time;
        private void OnEnable()
        {
            SmoothScale();
        }

        private void SmoothScale()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, _time);
        }
    }
}