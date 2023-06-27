using SaveSystem;

namespace PlayerData
{
    public abstract class BasePlayerData<T> where T : ISaveble, new()
    {
        private readonly string DATA_PATCH;
        private static T _data;
        private BinarySaveSystem _saveSystem;

        protected T Data => _data;

        protected BasePlayerData(string dataPatch)
        {
            DATA_PATCH = dataPatch;
        }

        protected void Load()
        {
            _saveSystem ??= new BinarySaveSystem(DATA_PATCH);
            _data = (T)_saveSystem.Load();
            if (_data == null)
            {
                _data = new T();
                Save();
            }
        }

        protected void Save()
        {
            _saveSystem.Save(_data);
        }
    }
}