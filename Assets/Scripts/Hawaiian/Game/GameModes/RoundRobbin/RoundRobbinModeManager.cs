using System.Collections.Generic;
using Hawaiian.Inventory;
using UnityEngine;

namespace Hawaiian.Game.GameModes.RoundRobbin
{
    [CreateAssetMenu(order = 0, menuName = "Hawaiian/Managers/GameModeManager/RoundRobbinModeManager")]
    public class RoundRobbinModeManager : ModeManager<RoundRobbinSceneReference>
    {
        private readonly Dictionary<PlayerConfig, InventoryController> inventoryControllers = new();
        private readonly Dictionary<PlayerConfig, Score> scores = new();
        
        protected override void OnPlayerJoined(PlayerConfig playerConfig)
        {
            // TODO: Duplicate code. See GameDialogue.OnPlayerJoined.
            var inventoryController = playerConfig.playerInput.GetComponentInChildren<InventoryController>();

            inventoryControllers.Add(playerConfig, inventoryController);

            // TODO: Remove listener
            inventoryController.inv.inventoryChanged.AddListener(UpdateWinningPlayers);
            
            // TODO: Duplicate code. See GameDialogue.OnPlayerJoined.
            var score = playerConfig.playerInput.GetComponentInChildren<Score>();

            scores.Add(playerConfig, score);
            
            base.OnPlayerJoined(playerConfig);
        }
        
        protected override float PlayerConfigScore(PlayerConfig playerConfig)
        {
            return scores[playerConfig].ScoreValue;
        }
    }
}