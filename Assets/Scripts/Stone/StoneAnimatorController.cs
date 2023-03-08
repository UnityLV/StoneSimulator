using System;
using System.Collections;
using Stone.Interfaces;
using UnityEngine;
using Zenject;

namespace Stone
{
    public class StoneAnimatorController : MonoBehaviour, IStoneAnimatorCallbackInvoke
    {
        private Coroutine _scaleCoroutine;
        [SerializeField]
        private Vector3 _initialScale;
        private const float DELTA_SCALE = 0.1f;
        private const int SCALE_TIME = 15;


        [SerializeField]
        private Animator _stoneAnimator;

        private static readonly int spawn = Animator.StringToHash("Spawn");
        private static readonly int click = Animator.StringToHash("Click");
        private static readonly int destroy = Animator.StringToHash("Destroy");
        private IStoneAnimatorEvents _stoneAnimatorEvents;
        private IStoneAnimatorCallbackInvoke _stoneAnimatorCallbackInvoke;

        #region Dependency

        [Inject]
        private void Construct(IStoneAnimatorEvents stoneAnimatorEvents,
            IStoneAnimatorCallbackInvoke stoneAnimatorEventsInvoke)
        {
            _stoneAnimatorEvents = stoneAnimatorEvents;
            _stoneAnimatorCallbackInvoke = stoneAnimatorEventsInvoke;
            _stoneAnimatorEvents.OnStoneClickPlay += ClickAnimationPlay;
            _stoneAnimatorEvents.OnStoneDestroyPlay += DestroyAnimationPlay;
            _stoneAnimatorEvents.OnStoneSpawnPlay += SpawnAnimationPlay;
        }

        #endregion


        private void ClickAnimationPlay()
        {
            // if (_scaleCoroutine != null) StopCoroutine(_scaleCoroutine);
            // _scaleCoroutine = StartCoroutine(IScaleAnimation());
            _stoneAnimator.SetTrigger(click);
        }

        private void SpawnAnimationPlay()
        {
            _stoneAnimator.SetTrigger(spawn);
        }

        private void DestroyAnimationPlay()
        {
            _stoneAnimator.SetTrigger(destroy);
        }

        private void OnDestroy()
        {
            _stoneAnimatorEvents.OnStoneClickPlay -= ClickAnimationPlay;
            _stoneAnimatorEvents.OnStoneDestroyPlay -= DestroyAnimationPlay;
            _stoneAnimatorEvents.OnStoneSpawnPlay -= SpawnAnimationPlay;
        }

        public void OnStoneDestroyedInvoke()
        {
            _stoneAnimatorCallbackInvoke.OnStoneDestroyedInvoke();
        }

        public void OnStoneClickedInvoke()
        {
            _stoneAnimatorCallbackInvoke.OnStoneClickedInvoke();
        }

        public void OnStoneSpawnedInvoke()
        {
            _stoneAnimatorCallbackInvoke.OnStoneSpawnedInvoke();
        }

        private IEnumerator IScaleAnimation()
        {
            Debug.Log($"Why we still here {_initialScale * (1 + DELTA_SCALE)}");
            yield return StartCoroutine(transform.ScaleWithLerp(_initialScale, _initialScale * (1 + DELTA_SCALE),
                SCALE_TIME));
            Debug.Log("Why we still here");
            yield return StartCoroutine(transform.ScaleWithLerp(_initialScale * (1 + DELTA_SCALE),
                _initialScale * (1 - DELTA_SCALE), SCALE_TIME));
            Debug.Log("Why we still here");
            yield return StartCoroutine(transform.ScaleWithLerp(_initialScale * (1 - DELTA_SCALE), _initialScale,
                SCALE_TIME));
            Debug.Log("Why we still here");
            _stoneAnimatorCallbackInvoke.OnStoneClickedInvoke();
        }
    }
}