using FirebaseCustom;
using MongoDBCustom;
using PlayerData.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace InGameUI
{
    public class SlaveRedeemed : MonoBehaviour
    {
        [SerializeField] private PlayerConfig _config;

        [SerializeField] private UnityEvent _onRedeemed;
        private IClickDataService _clickData;
        private IDBValues _values;
        
        
        [Inject]
        public void Construct(IClickDataService clickData, IDBValues values)
        {
            _clickData = clickData;
            _values = values;
        }
        
        public void TryRedeemed()
        {
            if (_clickData.GetClickCount() >= _config.ClicksToRedeemed)
            {
                _values.AddAllPlayerClicks(-_config.ClicksToRedeemed);
                _clickData.AddClicks(-_config.ClicksToRedeemed);
                Redeemed();
            }
        }

        private void Redeemed()
        {
            Debug.Log("Slave Redeemed");
            _onRedeemed?.Invoke();
            _values.RemoveMeFromReferral();
        }
    }
}