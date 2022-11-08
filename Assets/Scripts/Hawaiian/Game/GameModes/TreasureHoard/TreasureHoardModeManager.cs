using System.Collections.Generic;
using Hawaiian.Inventory;
using UnityEngine;

namespace Hawaiian.Game.GameModes.TreasureHoard
{
    [CreateAssetMenu(order = 0, menuName = "Hawaiian/Managers/GameModeManager/TreasureHoardModeManager")]
    public class TreasureHoardModeManager : ModeManager<TreasureHoardSceneReference>
    {
        public delegate void TreasureInitalised(PlayerConfig config);
        public TreasureInitalised OnTreasureInitialised;
        
        [Header("Treasure Hoard Mode Manager")]
        [SerializeField] private Item _depositor;
        [SerializeField] private Item _detonator;
        [SerializeField] private GameObject _playerTreasurePrefab;
        
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
            GameObject playerTreasureObject = Instantiate(_playerTreasurePrefab, sceneReference.treasureSpawnPoints[playerConfig.playerNumber], Quaternion.identity);

            var playerTreasure = playerTreasureObject.GetComponent<PlayerTreasure>();

            var playerInventoryController = playerManager.InventoryControllers[playerConfig];

          //  playerTreasure.Initialise(playerConfig.playerNumber, playerInventoryController);
            
            playerTreasures.Add(playerConfig, playerTreasure);
        
            playerInventoryController.inv.inv[0] = _depositor;
            playerInventoryController.inv.inv[1] = _detonator;

            base.OnPlayerJoined(playerConfig);
            
            playerTreasure.Owner = playerManager.LastPlayerJoined;
            OnTreasureInitialised.Invoke(playerConfig);
        }
    }
}