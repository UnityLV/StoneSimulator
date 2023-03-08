namespace GameScene.Interfaces
{
    public interface IGameSceneService
    {
        /// <summary>
        /// Не вызывать в Awake()!
        /// </summary>
        /// <param name="state"></param>
        public void BeginLoadGameScene(GameSceneType state);
        public void BeginTransaction();
    }
}