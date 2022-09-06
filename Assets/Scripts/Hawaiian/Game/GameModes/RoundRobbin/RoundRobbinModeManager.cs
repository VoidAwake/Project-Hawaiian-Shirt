using UnityEngine;

namespace Hawaiian.Game.GameModes.RoundRobbin
{
    [CreateAssetMenu(order = 0, menuName = "Hawaiian/Managers/GameModeManager/RoundRobbinModeManager")]
    public class RoundRobbinModeManager : ModeManager<RoundRobbinSceneReference>
    {
        public override void SaveScores()
        {
            base.SaveScores();
            
            foreach (var (playerConfig, inventoryController) in inventoryControllers)
            {
                playerConfig.score = inventoryController.Score;
            }
        }
    }
}