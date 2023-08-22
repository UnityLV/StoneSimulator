using System;
using System.Threading.Tasks;
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
        private void Construct(IStoneClickEvents stoneClickEvents, IDBCommands idbCommands, ISlaveClickCollector slaveClickCollector, IAbilityClickEvents abilityClickEvents)
        {
            _stoneClickEvents = stoneClickEvents;
            _idbCommands = idbCommands;
            _slaveClickCollector = slaveClickCollector;
            _aAbilityClickEvents = abilityClickEvents;
            _stoneClickEvents.OnStoneClick += OnStoneClick;
            _aAbilityClickEvents.OnAbilityClick += OnAbilityClick;
        }

        private void OnAbilityClick(int abilityClicks)
        {
            _clickToAdd += abilityClicks;
            Save();
        }


        private void OnStoneClick()
        {
            _clickToAdd++;
        }

        public async Task Save(int amount = 0)
        {
            _clickToAdd += amount;
            if (_clickToAdd == 0)
            {
                return;
            }

            await _idbCommands.AddAllPlayerClicks(_clickToAdd);

            _clickToAdd = 0;
        }

        public void Dispose()
        {
            _stoneClickEvents.OnStoneClick -= OnStoneClick;
         
            _aAbilityClickEvents.OnAbilityClick -= OnAbilityClick;
        }
    }
}