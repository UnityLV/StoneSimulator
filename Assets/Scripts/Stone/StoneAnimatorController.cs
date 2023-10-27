using System.Threading.Tasks;
using Stone.Interfaces;
using UnityEngine;
using Zenject;

namespace Stone
{
    public class StoneAnimatorController : MonoBehaviour, IStoneAnimatorCallbackInvoke
    {
        [SerializeField]
        private Animator _stoneAnimator;

        [SerializeField] private GameObject _spawnEffect;

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

        private void Start()
        {
            Debug.Log("SpawnAnimationPlay");
            Instantiate(_spawnEffect);
        }

        //Eta hueta ne vizivaetca, pochemy HZ
        private void SpawnAnimationPlay()
        {
            _stoneAnimator.SetTrigger(spawn);
        }

        private void DestroyAnimationPlay()
        {
            Debug.Log("DestroyAnimationPlay");
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
    }
}