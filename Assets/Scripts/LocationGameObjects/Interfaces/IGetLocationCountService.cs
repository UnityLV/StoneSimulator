namespace LocationGameObjects.Interfaces
{
    public interface IGetLocationCountService
    {
        public int GetLocationsCount();
        public int GetStoneCount(int location);
    }
}