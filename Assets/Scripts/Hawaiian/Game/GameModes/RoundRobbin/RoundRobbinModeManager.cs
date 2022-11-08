using System.Collections.Generic;
using Hawaiian.Inventory;
using UnityEngine;

namespace Hawaiian.Game.GameModes.RoundRobbin
{
    [CreateAssetMenu(order = 0, menuName = "Hawaiian/Managers/GameModeManager/RoundRobbinModeManager")]
    public class RoundRobbinModeManager : ModeManager<RoundRobbinSceneReference>
    {
        private readonly Dictionary<PlayerConfig, InventoryController> inventoryControllers = new();
        
        protected override void OnPlayerJoined(PlayerConfig playerConfig)
        {
            // TODO: Duplicate code. See GameDialogue.OnPlayerJoined.
            var inventoryController = playerConfig.playerInput.GetComponentInChildren<InventoryController>();

            inventoryControllers.Add(playerConfig, inventoryController);

            // TODO: Remove listener
            inventoryController.inv.inventoryChanged.AddListener(UpdateWinningPlayers);
            
            base.OnPlayerJoined(playerConfig);
        }
        
        protected override float PlayerConfigScore(PlayerConfig playerConfig)
        {
            return inventoryControllers[playerConfig].Score;
        }
    }
}