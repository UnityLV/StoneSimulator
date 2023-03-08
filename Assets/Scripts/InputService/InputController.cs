using System;
using System.Collections;
using InputService.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace InputService
{
    public class InputController : MonoBehaviour, IInputEvents
    {
        public event Action<Vector2> OnRotationSwipe;
        public event Action<float> OnZoomChange;

        private Coroutine _zoomCoroutine;
        private bool _isMouseDrag;

        private CameraInput _cameraInput;

        private void Awake()
        {
            _cameraInput = new CameraInput();
            _cameraInput.CameraTouch.Swipe.performed += context =>
            {
                if (Touch.activeTouches.Count == 1)
                    OnRotationSwipe?.Invoke(_cameraInput.CameraTouch.Swipe.ReadValue<Vector2>());
            };
            _cameraInput.CameraTouch.TouchContact.started += context => StartZoom();
            _cameraInput.CameraTouch.TouchContact.canceled += context => EndZoom();
            _cameraInput.CameraMouse.Zoom.performed += context =>
            {
                OnZoomChange?.Invoke(_cameraInput.CameraMouse.Zoom.ReadValue<float>() / 360f);
                Debug.Log(_cameraInput.CameraMouse.Zoom.ReadValue<float>() / 360f);
            };
            _cameraInput.CameraMouse.Drag.started += context => _isMouseDrag = true;
            _cameraInput.CameraMouse.Drag.canceled += context => _isMouseDrag = false;
            _cameraInput.CameraMouse.DragDelta.performed += context =>
            {
                if (_isMouseDrag) OnRotationSwipe?.Invoke(_cameraInput.CameraMouse.DragDelta.ReadValue<Vector2>());
            };
        }


        private void StartZoom()
        {
            _zoomCoroutine = StartCoroutine(IZoomProcess());
        }

        private void EndZoom()
        {
            StopCoroutine(_zoomCoroutine);
        }

        private IEnumerator IZoomProcess()
        {
            float currentMagnitude = (_cameraInput.CameraTouch.TouchOnePosition.ReadValue<Vector2>() -
                                      _cameraInput.CameraTouch.TouchTwoPosition.ReadValue<Vector2>()).magnitude;
            float prevMagnitude = currentMagnitude;
            ;

            while (true)
            {
                currentMagnitude = (_cameraInput.CameraTouch.TouchOnePosition.ReadValue<Vector2>() -
                                    _cameraInput.CameraTouch.TouchTwoPosition.ReadValue<Vector2>()).magnitude;
                if (currentMagnitude - prevMagnitude != 0)
                {
                    OnZoomChange?.Invoke(currentMagnitude - prevMagnitude);
                    prevMagnitude = currentMagnitude;
                }

                yield return null;
            }
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
            _cameraInput.Disable();
        }

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            _cameraInput.Enable();
        }
    }
}