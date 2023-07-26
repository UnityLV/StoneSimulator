using System;
using PlayerData.Interfaces;
using Stone.Interfaces;
using Zenject;

namespace MongoDBCustom
{
    public class AllClickSaver : IDBAllClickSaver, IDisposable
    {
        private IStoneClickEvents _stoneClickEvents;
        private ISlaveClickCollector _slaveClickCollector;
        private IDBValues _dbValues;
        
        private int _clickToAdd = 0;

        [Inject]
        private void Construct(IStoneClickEvents stoneClickEvents,IDBValues dbValues ,ISlaveClickCollector slaveClickCollector)
        {
            _stoneClickEvents = stoneClickEvents;
            _dbValues = dbValues;
            _slaveClickCollector = slaveClickCollector;
            _stoneClickEvents.OnStoneClick += OnStoneClick;
            _slaveClickCollector.Collected += OnClicksCollected;
        }

        private void OnClicksCollected(int collected)
        {
            _clickToAdd += collected;
            Save();
        }

        private void OnStoneClick(int _)
        {
            _clickToAdd++;
        }

        public void Save(int amount = 0)
        {
            _clickToAdd += amount;
            if (_clickToAdd == 0)
            {
                return;
            }
            
            _dbValues.AddAllPlayerClicks(_clickToAdd).ContinueWith(
                (task)=> _clickToAdd = 0);
        }

        public void Dispose()
        {
            _stoneClickEvents.OnStoneClick -= OnStoneClick;
            _slaveClickCollector.Collected -= OnClicksCollected;
        }
    }
}