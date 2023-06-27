using System;
using Stone.Interfaces;
using Zenject;

namespace MongoDBCustom
{
    public class AllClickSaver : IDBAllClickSaver, IDisposable
    {
        private IStoneClickEvents _stoneClickEvents;
        private IDBValues _dbValues;
        
        private int _clickCount = 0;

        [Inject]
        private void Construct(IStoneClickEvents stoneClickEvents,IDBValues dbValues)
        {
            _stoneClickEvents = stoneClickEvents;
            _dbValues = dbValues;
            _stoneClickEvents.OnStoneClick += OnStoneClick;
        }

        private void OnStoneClick()
        {
            _clickCount++;
        }

        public void Save()
        {
            if (_clickCount == 0)
            {
                return;
            }
            _dbValues.AddAllPlayerClicks(_clickCount).ContinueWith(
                (task)=> _clickCount = 0);
        }

        public void Dispose()
        {
            _stoneClickEvents.OnStoneClick -= OnStoneClick;
        }
    }
}