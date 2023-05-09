using CameraRotation.Interfaces;
using InputService.Interfaces;
using UnityEngine;
using Zenject;

namespace CameraRotation
{
    public class CameraRotationObject : MonoBehaviour, IChangeTargetObject
    {
        private Transform _targetObject;

        [SerializeField, Header("Rotation properties")]
        private float _offset;

        [SerializeField, Range(-5, 5)]
        private float _rotationSensitivity;

        [SerializeField, Range(1, 5), Header("Zoom properties")]
        private float _minZoom;

        [SerializeField, Range(5, 50)]
        private float _maxZoom;

        [SerializeField, Range(-5, 5)]
        private float _zoomSensitivity;

        private const int COUNT_FRAME_LERP_POSITION = 25;
        private bool _isCameraLocked = false;

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
            if (_isCameraLocked) return;

            transform.localEulerAngles += new Vector3(-value.y, value.x, 0) * _rotationSensitivity;
            UpdatePosition();
        }

        private void OnZoomChange(float value)
        {
            if (_isCameraLocked) return;
            _offset = Mathf.Clamp(_offset + value * _zoomSensitivity, _minZoom, _maxZoom);
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (_isCameraLocked) return;
            transform.position =
                ((_targetObject) ? _targetObject.position : Vector3.zero) - transform.forward * _offset;
        }

        public void ChangeTargetPosition(Transform target)
        {
            if (_targetObject != null && _targetObject == target) return;
            _targetObject?.gameObject.SetActive(true);
            _targetObject = target;
            _targetObject.gameObject.SetActive(false);
            StopAllCoroutines();
            _isCameraLocked = true;
            StartCoroutine(transform.PositionWithLerp(transform.position,
                _targetObject.position - transform.forward * _offset, COUNT_FRAME_LERP_POSITION,
                delegate { _isCameraLocked = false; }));
        }
    }
}