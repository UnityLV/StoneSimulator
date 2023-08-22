using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MainMenuUI
{
    public class CollectSlaveClicksButton : MonoBehaviour
    {
        private ISlaveClickCollector _slaveClickCollector;

        [Inject]
        private void Construct(ISlaveClickCollector slaveClickCollector)
        {
            _slaveClickCollector = slaveClickCollector;
        }

        private async void Awake()
        {
            await Task.Delay(100);
            Collect();
        }

        public void Collect()
        {
            _slaveClickCollector.CollectClicksFromReferrals();
        }
    }
}