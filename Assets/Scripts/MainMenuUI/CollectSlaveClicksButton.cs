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
        
        public void Collect()
        {
            _slaveClickCollector.CollectClicksFromReferrals();
        }
    }
}