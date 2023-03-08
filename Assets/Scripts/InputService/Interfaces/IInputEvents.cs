using System;
using UnityEngine;

namespace InputService.Interfaces
{
    public interface IInputEvents
    {
        public event Action<Vector2> OnRotationSwipe;
        public event Action<float> OnZoomChange;
    }
}