using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace InGameUI
{
    public class UIMoveButton : MonoBehaviour
    {
        [SerializeField] private Vector3 _inPosition;
        [SerializeField] private float _inTime;
        [SerializeField] private Ease _inEase;

        [SerializeField] private float _outTime;
        [SerializeField] private Ease _outEase;

        [SerializeField] private UnityEvent _onMoveIn;
        [SerializeField] private UnityEvent _onMoveOut;

        private bool _isOut = true;

        private Vector3 _toMovePosition;
        private Vector3 _defaultPosition;

        private void Start()
        {
            _defaultPosition = transform.localPosition;
            _toMovePosition = _defaultPosition + _inPosition;
        }


        [Button()]
        public virtual void Move()
        {
            transform.DOKill();
            if (_isOut)
            {
                MoveIn();
            }
            else
            {
                MoveOut();
            }

            _isOut = !_isOut;
        }

        private async void MoveIn()
        {
            _onMoveIn?.Invoke();
            await transform.DOLocalMove(_toMovePosition, _inTime).SetEase(_inEase).AsyncWaitForCompletion();
        }

        private async void MoveOut()
        {
            _onMoveOut?.Invoke();
            await transform.DOLocalMove(_defaultPosition, _outTime).SetEase(_outEase).AsyncWaitForCompletion();
        }
    }
}