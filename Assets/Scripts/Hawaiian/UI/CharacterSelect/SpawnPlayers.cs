using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hawaiian.Game;
using Hawaiian.Inventory;
using Hawaiian.Utilities;
using MoreLinq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Hawaiian.UI.CharacterSelect
{
    public class SpawnPlayers : MonoBehaviour
    {
        public enum CharacterNames { Fox, Robin, Monkey, Cat, Goose, Soup, Gambit, Bert }
        enum PlayerColours { Red, Blue, Yellow, Green }

        [SerializeField] GameObject playerPrefab;
        [SerializeField] private int buildIndex;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameEvent playersJoined;
        [SerializeField] private List<SpawnPoint> spawnPoints;
        
        private Dictionary<LobbyGameManager.PlayerConfig, InventoryController> inventoryControllers = new();

        private LobbyGameManager lobbyManager;
        private PlayerInputManager inputManager;

        void Start()
        {
            lobbyManager = FindObjectOfType<LobbyGameManager>();
             inputManager = GetComponent<PlayerInputManager>();

            if (lobbyManager == null || inputManager == null)
            {
                if (inputManager != null)
                {
                    inputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;

                    inputManager.onPlayerJoined += OnPlayerJoined;
                }

                return;
            }
            
            
            inputManager.onPlayerJoined += OnPlayerJoined;
            inputManager.playerPrefab = playerPrefab;

          
            inputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
        }


        public void BeginSpawn()
        {
            foreach (LobbyGameManager.PlayerConfig config in lobbyManager.playerConfigs)
            {
                if (!config.IsPlayer) continue;
                
                PlayerInput newPlayer = inputManager.JoinPlayer(config.playerIndex, config.splitScreenIndex,
                    config.controlScheme, config.deviceIds.Select(InputSystem.GetDeviceById).ToArray());

                // Update player character
                //Debug.Log("Spawn in player! charNo = " + config.characterNumber + ", charName = " + ((CharacterNames)config.characterNumber).ToString()
                //+ ". playNo = " + config.playerNumber + ", playColour = " + ((PlayerColours)config.playerNumber).ToString());
                newPlayer.GetComponent<Unit.Unit>().SetSpriteResolvers(
                    ((CharacterNames)config.characterNumber).ToString(),
                    ((PlayerColours)config.playerNumber).ToString());

                newPlayer.transform.position = spawnPoints[newPlayer.playerIndex].GetSpawnPosition();

                // Update inventory UI
                // Find inventory referenced by inventory controller contained by player prefab
                for (int i = 0; i < newPlayer.transform.childCount; i++)
                {
                    Inventory.InventoryController temp = newPlayer.transform.GetChild(i)
                        .GetComponent<Inventory.InventoryController>();

                    if (temp == null) continue;
                    
                    inventoryControllers.Add(config, temp);

                    Inventory.Inventory inv = temp._inv;

                    // Search for inventory UIs, and find the one that matches our inventory
                    Game.InventoryUI[] inventoryUIs = FindObjectsOfType<Game.InventoryUI>();
                    foreach (Game.InventoryUI inventoryUI in inventoryUIs)
                    {
                        if (inventoryUI.inv == inv)
                        {
                            inventoryUI.SetCharacterPortrait(config.characterNumber, config.playerNumber);
                        }
                    }
                }
            }

            // Data is still needed for results
            // Destroy(playerData.gameObject);
            playersJoined.Raise();

        }
        
        
        
        

        public void SaveScores()
        {
            if (gameManager.Phase != GameManager.GamePhase.GameOver) return;
            
            foreach (var VARIABLE in inventoryControllers)
            {
                var inventoryController = VARIABLE.Value;
                var playerConfig = VARIABLE.Key;

                // TODO: Duplicate code. See ScoreUI.
                var score = inventoryController._inv.inv.Where(i => i != null).Sum(i => i.Points);

                playerConfig.score = score;
            }
            
            Transition transition = FindObjectOfType<Transition>();
            if (transition != null)
            {
                transition.BeginTransition(true, true, buildIndex, false);
            }
            else
            {
                SceneManager.LoadScene(buildIndex);
            }
        }

        // Message from Player Input Manager
        private void OnPlayerJoined(PlayerInput playerInput)
        {
            playersJoined.Raise();
        }
        
        public Transform WinningPlayer { get; private set; }

        private void Update()
        {
            if (inventoryControllers.Count == 0) return;
            
            WinningPlayer = inventoryControllers.MaxBy(i => InventoryScore(i.Value)).Value.transform;
        }

        private static float InventoryScore(InventoryController inventoryController)
        {
            return inventoryController._inv.inv.Where(i => i != null).Sum(i => i.Points);
        }
    }
}
