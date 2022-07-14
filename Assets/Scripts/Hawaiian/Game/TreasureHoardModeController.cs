using System;
using System.Collections.Generic;
using Hawaiian.Inventory;
using Hawaiian.Utilities;
using UnityEngine;

namespace Hawaiian.Game
{
    public class TreasureHoardModeController : MonoBehaviour, IModeController
    {
        [SerializeField] private Item _depositor;
        [SerializeField] private Item _detonator;
        [SerializeField] private GameObject _playerTreasurePrefab;
        [SerializeField] private GameEvent playerTreasureCreated;
        
        private Dictionary<PlayerConfig, PlayerTreasure> playerTreasures = new();

        public Dictionary<PlayerConfig, PlayerTreasure> PlayerTreasures => new(playerTreasures);

        private PlayerManager playerManager;
        private TreasureHoardSceneReference sceneReference;

        private void OnDestroy()
        {
            if (playerManager != null)
                playerManager.playerJoined.RemoveListener(OnPlayerJoined);
        }

        public void Initialise(PlayerManager playerManager, GameModeSceneReference gameModeSceneReference)
        {
            this.playerManager = playerManager;
            
            playerManager.playerJoined.AddListener(OnPlayerJoined);

            sceneReference = (TreasureHoardSceneReference) gameModeSceneReference;
        }

        public void SaveScores()
        {
            foreach (var (playerConfig, playerTreasure) in playerTreasures)
            {
                playerConfig.score = playerTreasure.CurrentPoints;
            }
        }

        public void OnPlayerJoined(PlayerConfig playerConfig)
        {
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