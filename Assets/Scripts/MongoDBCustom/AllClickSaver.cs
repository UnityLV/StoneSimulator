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
        private IDBCommands _idbCommands;
        
        private int _clickToAdd = 0;
        private IAbilityClickEvents _aAbilityClickEvents;

        [Inject]
        private void Construct(IStoneClickEvents stoneClickEvents,IDBCommands idbCommands ,ISlaveClickCollector slaveClickCollector,IAbilityClickEvents abilityClickEvents)
        {
            _stoneClickEvents = stoneClickEvents;
            _idbCommands = idbCommands;
            _slaveClickCollector = slaveClickCollector;
            _aAbilityClickEvents = abilityClickEvents;
            _stoneClickEvents.OnStoneClick += OnStoneClick;
            _slaveClickCollector.Collected += OnClicksCollected;
            _aAbilityClickEvents.OnAbilityClick += OnAbilityClick;
        }

        private void OnAbilityClick(int abilityClicks)
        {
            _clickToAdd += abilityClicks;
            Save();
        }

        private void OnClicksCollected(int collected)
        {
            _clickToAdd += collected;
            Save();
        }

        private void OnStoneClick()
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
            
            _idbCommands.AddAllPlayerClicks(_clickToAdd);
            
            _clickToAdd = 0;
        }

        public void Dispose()
        {
            _stoneClickEvents.OnStoneClick -= OnStoneClick;
            _slaveClickCollector.Collected -= OnClicksCollected;
            _aAbilityClickEvents.OnAbilityClick -= OnAbilityClick;
        }
    }
}