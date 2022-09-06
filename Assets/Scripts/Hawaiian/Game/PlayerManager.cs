using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Hawaiian.Game
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameEvent playersJoined;
        [SerializeField] private List<SpawnPoint> spawnPoints;
        [SerializeField] private PlayerConfigManager playerConfigManager;

        public UnityEvent<PlayerConfig> playerJoined = new();
        
        private PlayerInputManager inputManager;

        private bool playerInputEnabled;

        private List<PlayerInput> _allPlayers = new List<PlayerInput>();
        
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
                playerConfig.playerInput = playerInput;
                
                playerInput.GetComponent<UnitPlayer>().Initialise(playerConfig.characterNumber, playerConfig.playerNumber);

                playerInput.transform.position = spawnPoints[playerInput.playerIndex].GetSpawnPosition();
            }
            else
            {
                Debug.LogWarning($"Unable to fully initialise player. No {nameof(PlayerConfig)}.");
            }
            
            playerJoined.Invoke(playerConfig);

            playersJoined.Raise();
        }

        public void AllowAllInputs()
        {
            _allPlayers.ForEach(input => { input.ActivateInput(); });

            playerInputEnabled = true;
        }   
    }
}
