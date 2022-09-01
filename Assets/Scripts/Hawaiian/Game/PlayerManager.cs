using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Hawaiian.Inventory;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Hawaiian.Game
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameEvent playersJoined;
        [SerializeField] private List<SpawnPoint> spawnPoints;
        [SerializeField] private PlayerConfigManager playerConfigManager;
        [SerializeField] private BaseGameEvent<Inventory.Inventory> addedInventory;

        public UnityEvent winningPlayersChanged = new();
        public UnityEvent<PlayerConfig> playerJoined = new();
        
        private Dictionary<PlayerConfig, InventoryController> inventoryControllers = new();

        private PlayerInputManager inputManager;

        private bool playerInputEnabled;

        public ReadOnlyDictionary<PlayerConfig, InventoryController> InventoryControllers => new (inventoryControllers);

        private List<PlayerInput> _allPlayers = new List<PlayerInput>();
        
        public UnitPlayer LastPlayerJoined { get; private set; }

        private void Awake()
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
            StartCoroutine(BeginSpawnCoroutine());
        }
        
        private IEnumerator BeginSpawnCoroutine() {
            if (playerConfigManager == null) yield break;
            
            foreach (PlayerConfig playerConfig in playerConfigManager.playerConfigs)
            {
                yield return StartCoroutine(SpawnPlayerCoroutine(playerConfig));
                // TODO: Magic number.
                yield return new WaitForSeconds(0.4f);
            }
        }

        private IEnumerator SpawnPlayerCoroutine(PlayerConfig playerConfig)
        {
            if (!playerConfig.IsPlayer) yield break;
            
            PlayerInput playerInput = inputManager.JoinPlayer(
                playerConfig.playerIndex,
                playerConfig.splitScreenIndex,
                playerConfig.controlScheme,
                playerConfig.deviceIds.Select(InputSystem.GetDeviceById).ToArray()
            );

            OnPlayerJoined(playerInput, playerConfig);
        }

        // Message from Player Input Manager
        private void OnPlayerJoined(PlayerInput playerInput)
        {
            // TODO: Create a new PlayerConfig with a unique character and player number
            OnPlayerJoined(playerInput, null);    
        }
        
        private void OnPlayerJoined(PlayerInput playerInput, PlayerConfig playerConfig)
        {
            // TODO: No idea if this is where this is meant to go
            _allPlayers.Add(playerInput);
            
            if (!playerInputEnabled)
                playerInput.DeactivateInput();
            
            if (playerConfig != null)
            {
                playerInput.GetComponent<UnitPlayer>().Initialise(playerConfig.characterNumber, playerConfig.playerNumber);

                playerInput.transform.position = spawnPoints[playerInput.playerIndex].GetSpawnPosition();

                var inventoryController = playerInput.GetComponentInChildren<InventoryController>();

                if (inventoryController != null)
                {
                    inventoryControllers.Add(playerConfig, inventoryController);
                    
                    inventoryController.currentItemChanged.AddListener(UpdateWinningPlayers);

                    addedInventory.Raise(inventoryController.inv);
                }

                LastPlayerJoined = playerInput.GetComponent<UnitPlayer>();
            }
            else
            {
                Debug.LogWarning($"Unable to fully initialise player. No {nameof(PlayerConfig)}.");
            }
            
            playerJoined.Invoke(playerConfig);

            playersJoined.Raise();
        }

        public List<Transform> WinningPlayers { get; private set; }

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
            return inventoryController.inv.inv.Where(i => i != null).Sum(i => i.Points);
        }

        public PlayerConfig GetPlayerConfig(Inventory.Inventory inv)
        {
            return inventoryControllers.FirstOrDefault(a => a.Value.inv == inv).Key;
        }

        public void AllowAllInputs()
        {
            _allPlayers.ForEach(input => { input.ActivateInput(); });

            playerInputEnabled = true;
        }   
    }
}
