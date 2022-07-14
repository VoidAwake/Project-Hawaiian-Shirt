namespace Hawaiian.Game
{
    public interface IModeController
    {
        public void Initialise(PlayerManager playerManager, GameModeSceneReference gameModeSceneReference);
        
        public void SaveScores();

        public void OnPlayerJoined(PlayerConfig playerConfig);
    }
}