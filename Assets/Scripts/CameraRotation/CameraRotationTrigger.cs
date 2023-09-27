using CameraRotation.Interfaces;
using Zenject;
using UnityEngine;

namespace CameraRotation
{
    public class CameraRotationTrigger : MonoBehaviour
    {
        #region Dependency

        private IChangeTargetObject _changeTargetObject;

        [Inject]
        private void Construct(IChangeTargetObject changeTargetObject)
        {
            _changeTargetObject = changeTargetObject;
        }

        #endregion


        [SerializeField]
        private bool _isStarted;

        private void Start()
        {
            if (_isStarted) OnClick();
        }

        public void OnClick()
        {
            _changeTargetObject.ChangeTargetPosition(this.transform);
        }
    }
}