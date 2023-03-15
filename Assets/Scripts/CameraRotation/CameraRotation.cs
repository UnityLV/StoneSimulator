using InputService.Interfaces;
using Stone.Interfaces;
using UnityEngine;
using Zenject;

namespace CameraRotation
{
    public class CameraRotation : MonoBehaviour
    {
        [SerializeField]
        private Transform _targetObject;

        [SerializeField, Header("Rotation properties")]
        private float _offset;

        [SerializeField, Range(-5,5)]
        private float _rotationSensitivity;

        [SerializeField, Range(1, 5), Header("Zoom properties")]
        private float _minZoom;
        
        [SerializeField, Range(5, 50)]
        private float _maxZoom;
        
        [SerializeField, Range(-5,5)]
        private float _zoomSensitivity;
        

        #region Dependency

        [Inject]
        private void Construct(IInputEvents inputEvents)
        {
            inputEvents.OnRotationSwipe += OnRotationChange;
            inputEvents.OnZoomChange += OnZoomChange;
        }

        #endregion

        private void OnRotationChange(Vector2 value)
        {
            transform.localEulerAngles += new Vector3(-value.y,value.x,0)*_rotationSensitivity;
            UpdatePosition();
        }
        
        private void OnZoomChange(float value)
        {
            _offset = Mathf.Clamp(_offset + value*_zoomSensitivity, _minZoom, _maxZoom);
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            transform.position = /*_targetObject.position*/ - transform.forward * _offset;
        }
    }
}