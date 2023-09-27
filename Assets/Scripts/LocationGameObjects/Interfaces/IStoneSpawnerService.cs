namespace LocationGameObjects.Interfaces
{
    public interface IStoneSpawnerService
    {
        public void SpawnStoneObject(int location, int stoneLvl);
        public void DestroyStoneObject(bool force);
    }
}