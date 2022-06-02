using System.Collections.Generic;
using System.Linq;
using Hawaiian.Inventory;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Hawaiian.Game
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] GameObject playerPrefab;
        [SerializeField] private int buildIndex;
        [SerializeField] private GameEvent playersJoined;
        [SerializeField] private List<SpawnPoint> spawnPoints;
        [SerializeField] private PlayerConfigManager playerConfigManager;
        [SerializeField] private BaseGameEvent<Inventory.Inventory> addedInventory;

        public UnityEvent winningPlayersChanged = new();
        
        private Dictionary<PlayerConfig, InventoryController> inventoryControllers = new();

        private PlayerInputManager inputManager;

        void Start()
        {
            inputManager = GetComponent<PlayerInputManager>();

            if (inputManager == null) return;

            inputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;

            inputManager.onPlayerJoined += OnPlayerJoined;

            if (playerConfigManager == null) return;

            inputManager.playerPrefab = playerPrefab;

            inputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
            
            inputManager.onPlayerJoined -= OnPlayerJoined;
        }

        public void BeginSpawn()
        {
            if (playerConfigManager == null) return;
            
            foreach (PlayerConfig playerConfig in playerConfigManager.playerConfigs)
            {
                if (!playerConfig.IsPlayer) continue;
                
                PlayerInput playerInput = inputManager.JoinPlayer(
                    playerConfig.playerIndex,
                    playerConfig.splitScreenIndex,
                    playerConfig.controlScheme,
                    playerConfig.deviceIds.Select(InputSystem.GetDeviceById).ToArray()
                );

                OnPlayerJoined(playerInput, playerConfig);
            }
        }

        public void SaveScores()
        {
            foreach (var (playerConfig, inventoryController) in inventoryControllers)
            {
                playerConfig.score = inventoryController.Score;
            }
            
            // TODO: Refactor. A function called SaveScores should not also be handling changing the scene.
            // TODO: Changing the scene should also not be handled by a PlayerManager.
            Transition transition = FindObjectOfType<Transition>();
            if (transition != null)
            {
                transition.BeginTransition(true, true, buildIndex);
            }
            else
            {
                SceneManager.LoadScene(buildIndex);
            }
        }

        // Message from Player Input Manager
        private void OnPlayerJoined(PlayerInput playerInput)
        {
            // TODO: Create a new PlayerConfig with a unique character and player number
            OnPlayerJoined(playerInput, null);    
        }
        
        private void OnPlayerJoined(PlayerInput playerInput, PlayerConfig playerConfig)
        {
            if (playerConfig != null)
            {
                playerInput.GetComponent<UnitPlayer>().Initialise(playerConfig.characterNumber, playerConfig.playerNumber);

                playerInput.transform.position = spawnPoints[playerInput.playerIndex].GetSpawnPosition();

                var inventoryController = playerInput.GetComponentInChildren<InventoryController>();

                if (inventoryController != null)
                {
                    inventoryControllers.Add(playerConfig, inventoryController);

                    addedInventory.Raise(inventoryController._inv);
                }
            }
            else
            {
                Debug.LogWarning($"Unable to fully initialise player. No {nameof(PlayerConfig)}.");
            }

            playersJoined.Raise();
        }
        
        public List<Transform> WinningPlayers { get; private set; }

        private void Update()
        {
            if (inventoryControllers.Count == 0) return;
            
            // TODO: Inefficient to be doing this every frame. Only needs to be updated when scores change.
            UpdateWinningPlayers();
        }

        private void UpdateWinningPlayers()
        {
            var inventoryScores = new Dictionary<InventoryController, float>();

            var maxScore = 0f;

            foreach (var (_, inventoryController) in inventoryControllers)
            {
                var score = InventoryScore(inventoryController);

                inventoryScores.Add(inventoryController, score);

                if (maxScore < score)
                    maxScore = score;
            }

            WinningPlayers = inventoryScores
                .Where(o => o.Value == maxScore)
                .Select(o => o.Key.transform)
                .ToList();
            
            if (WinningPlayers.Count == inventoryControllers.Count)
                WinningPlayers.Clear();
            
            winningPlayersChanged.Invoke();
        }

        private static float InventoryScore(InventoryController inventoryController)
        {
            return inventoryController._inv.inv.Where(i => i != null).Sum(i => i.Points);
        }

        public PlayerConfig GetPlayerConfig(Inventory.Inventory inv)
        {
            return inventoryControllers.FirstOrDefault(a => a.Value._inv == inv).Key;
        }
    }
}
