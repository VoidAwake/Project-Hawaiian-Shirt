namespace Hawaiian.Game.GameModes.RoundRobbin
{
    public class RoundRobbinModeController : ModeController<RoundRobbinSceneReference>
    {
        public override void SaveScores()
        {
            base.SaveScores();
            
            foreach (var (playerConfig, inventoryController) in playerManager.InventoryControllers)
            {
                playerConfig.score = inventoryController.Score;
            }
        }
    }
}