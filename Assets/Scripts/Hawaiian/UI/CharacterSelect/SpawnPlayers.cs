using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hawaiian.Game;
using Hawaiian.Inventory;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Hawaiian.UI.CharacterSelect
{
    public class SpawnPlayers : MonoBehaviour
    {
        enum CharacterNames { Fox, Robin, Monkey, Cat, Goose, Soup }
        enum PlayerColours { Red, Blue, Yellow, Green }

        [SerializeField] GameObject playerPrefab;
        [SerializeField] private int buildIndex;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameEvent playersJoined;
        private Dictionary<LobbyGameManager.PlayerConfig, InventoryController> inventoryControllers = new();

        // Start is called before the first frame update
        void Start()
        {
            LobbyGameManager playerData = FindObjectOfType<LobbyGameManager>();
            PlayerInputManager inputManager = GetComponent<PlayerInputManager>();

            if (playerData != null && inputManager != null)
            {
                inputManager.playerPrefab = playerPrefab;

                foreach (LobbyGameManager.PlayerConfig config in playerData.playerConfigs)
                {
                    if (config.isPlayer)
                    {
                        PlayerInput newPlayer = inputManager.JoinPlayer(config.playerIndex, config.splitScreenIndex, config.controlScheme, config.deviceIds.Select(InputSystem.GetDeviceById).ToArray());

                        // Update player character
                        //Debug.Log("Spawn in player! charNo = " + config.characterNumber + ", charName = " + ((CharacterNames)config.characterNumber).ToString()
                            //+ ". playNo = " + config.playerNumber + ", playColour = " + ((PlayerColours)config.playerNumber).ToString());
                        newPlayer.GetComponent<Unit.Unit>().SetSpriteResolvers(((CharacterNames)config.characterNumber).ToString(), ((PlayerColours)config.playerNumber).ToString());

                        // Update inventory UI
                        // Find inventory referenced by inventory controller contained by player prefab
                        for (int i = 0; i < newPlayer.transform.childCount; i++)
                        {
                            Inventory.InventoryController temp = newPlayer.transform.GetChild(i).GetComponent<Inventory.InventoryController>();
                            if (temp != null)
                            {
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
                    }
                }

                // Data is still needed for results
                // Destroy(playerData.gameObject);
                playersJoined.Raise();
                inputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
            }
            else if (inputManager != null)
            {
                inputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
            }
        }

        public void SaveScores()
        {
            if (gameManager.Phase != GameManager.GamePhase.GameOver) return;
            
            // TODO: For each player, get the score and save it to the PlayerConfig
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
    }
}
