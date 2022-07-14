using UnityEngine;

namespace Hawaiian.Game
{
    public class RoundRobbinModeController : MonoBehaviour, IModeController
    {
        private PlayerManager playerManager;

        private void OnDestroy()
        {
            if (playerManager != null)
                playerManager.playerJoined.RemoveListener(OnPlayerJoined);
        }

        public void Initialise(PlayerManager playerManager, GameModeSceneReference gameModeSceneReference)
        {
            this.playerManager = playerManager;
            
            playerManager.playerJoined.AddListener(OnPlayerJoined);
        }

        public void SaveScores()
        {
            foreach (var (playerConfig, inventoryController) in playerManager.InventoryControllers)
            {
                playerConfig.score = inventoryController.Score;
            }
        }

        public void OnPlayerJoined(PlayerConfig playerConfig) { }
    }
}