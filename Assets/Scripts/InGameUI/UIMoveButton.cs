﻿using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace InGameUI
{
    public class UIMoveButton : MonoBehaviour
    {
        [SerializeField] private Vector3 _inPosition;
        [SerializeField] private float _inTime;
        [SerializeField] private Ease _inEase; 
        
        [SerializeField] private float _outTime;
        [SerializeField] private Ease _outEase;

        private bool _isOut = true;

        private Vector3 _toMovePosition;
        private Vector3 _defaultPosition;

        private void Awake()
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
            await transform.DOLocalMove(_toMovePosition, _inTime).SetEase(_inEase).AsyncWaitForCompletion();
            
        }
        
        private async void MoveOut()
        {
            await transform.DOLocalMove(_defaultPosition, _outTime).SetEase(_outEase).AsyncWaitForCompletion();
          
        }
        
        
    }
}