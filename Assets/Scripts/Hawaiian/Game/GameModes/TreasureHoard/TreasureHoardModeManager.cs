using System.Collections.Generic;
using Hawaiian.Inventory;
using UnityEngine;

namespace Hawaiian.Game.GameModes.TreasureHoard
{
    [CreateAssetMenu(order = 0, menuName = "Hawaiian/Managers/GameModeManager/TreasureHoardModeManager")]
    public class TreasureHoardModeManager : ModeManager<TreasureHoardSceneReference>
    {
        [Header("Treasure Hoard Mode Manager")]
        [SerializeField] private Item _depositor;
        [SerializeField] private Item _detonator;
        [SerializeField] private GameObject _playerTreasurePrefab;
        
        private readonly Dictionary<PlayerConfig, PlayerTreasure> playerTreasures = new();
        private readonly Dictionary<PlayerConfig, InventoryController> inventoryControllers = new();
        
        public Dictionary<PlayerConfig, PlayerTreasure> PlayerTreasures => new(playerTreasures);

        protected override void OnPlayerJoined(PlayerConfig playerConfig)
        {
            // TODO: Duplicate code. See GameDialogue.OnPlayerJoined.
            var inventoryController = playerConfig.playerInput.GetComponentInChildren<InventoryController>();
            
            inventoryControllers.Add(playerConfig, inventoryController);

            GameObject playerTreasureObject = Instantiate(_playerTreasurePrefab, sceneReference.treasureSpawnPoints[playerConfig.playerNumber], Quaternion.identity);

            var playerTreasure = playerTreasureObject.GetComponent<PlayerTreasure>();
            
            // TODO: Remove listener
            playerTreasure.pointsChanged.AddListener(UpdateWinningPlayers);

            playerTreasure.Initialise(playerConfig.playerNumber, inventoryController);
            
            playerTreasures.Add(playerConfig, playerTreasure);
        
            inventoryController.inv.inv[0] = _depositor;
            inventoryController.inv.inv[1] = _detonator;
            
            base.OnPlayerJoined(playerConfig);
        }

        protected override float PlayerConfigScore(PlayerConfig playerConfig)
        {
            return playerTreasures[playerConfig].CurrentPoints;
        }
    }
}