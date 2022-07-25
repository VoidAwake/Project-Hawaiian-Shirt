using System.Collections.Generic;
using Hawaiian.Inventory;
using Hawaiian.Utilities;
using UnityEngine;

namespace Hawaiian.Game.GameModes.TreasureHoard
{
    public class TreasureHoardModeController : ModeController<TreasureHoardSceneReference>
    {
        [SerializeField] private Item _depositor;
        [SerializeField] private Item _detonator;
        [SerializeField] private GameObject _playerTreasurePrefab;
        [SerializeField] private GameEvent playerTreasureCreated;
        
        private Dictionary<PlayerConfig, PlayerTreasure> playerTreasures = new();

        public Dictionary<PlayerConfig, PlayerTreasure> PlayerTreasures => new(playerTreasures);

        public override void SaveScores()
        {
            base.SaveScores();
            
            foreach (var (playerConfig, playerTreasure) in playerTreasures)
            {
                playerConfig.score = playerTreasure.CurrentPoints;
            }
        }

        protected override void OnPlayerJoined(PlayerConfig playerConfig)
        {
            base.OnPlayerJoined(playerConfig);
            
            GameObject playerTreasureObject = Instantiate(_playerTreasurePrefab, sceneReference.treasureSpawnPoints[playerConfig.playerNumber], Quaternion.identity);

            var playerTreasure = playerTreasureObject.GetComponent<PlayerTreasure>();

            var playerInventoryController = playerManager.InventoryControllers[playerConfig];

            playerTreasure.Initialise(playerConfig.playerNumber, playerInventoryController);
            
            playerTreasures.Add(playerConfig, playerTreasure);
        
            playerInventoryController.inv.inv[0] = _depositor;
            playerInventoryController.inv.inv[1] = _detonator;

            playerTreasureCreated.Raise();
        }
    }
}