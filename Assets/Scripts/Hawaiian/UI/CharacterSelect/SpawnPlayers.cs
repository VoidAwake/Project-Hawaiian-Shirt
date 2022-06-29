using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hawaiian.Game;
using Hawaiian.Inventory;
using Hawaiian.UI.Game;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using MoreLinq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Hawaiian.UI.CharacterSelect
{
    public class SpawnPlayers : MonoBehaviour
    {
        public enum CharacterNames
        {
            Fox,
            Robin,
            Monkey,
            Cat,
            Goose,
            Soup,
            Gambit,
            Bert
        }

        enum PlayerColours
        {
            Red,
            Blue,
            Yellow,
            Green
        }
        [SerializeField] private Item _depositor;
        [SerializeField] private Item _detonator;

        [SerializeField] GameObject playerPrefab;
        [SerializeField] private int buildIndex;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameEvent playersJoined;
        [SerializeField] private List<SpawnPoint> spawnPoints;
        [SerializeField] private GameObject _playerTreasurePrefab;
        [SerializeField] private Vector3[] _playerTreasureSpawnPoint;

        [SerializeField] private GameObject _spawnEffectPrefab;

        public UnityEvent winningPlayersChanged = new();

        private Dictionary<PlayerConfig, InventoryController> inventoryControllers = new();

        private List<PlayerInput> _allPlayers = new List<PlayerInput>();
        
        private LobbyGameManager lobbyManager;
        private TreasureHoardUI _treasureHoardUI;
        private PlayerInputManager inputManager;

        public List<PlayerTreasure> _treasures = new List<PlayerTreasure>();

        void Start()
        {
            lobbyManager = FindObjectOfType<LobbyGameManager>();
            _treasureHoardUI = FindObjectOfType<TreasureHoardUI>();

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

        private Color32 GetPlayerColour(PlayerColours color)
        {
            switch (color)
            {
                case PlayerColours.Blue:
                    return Color.blue;
                case PlayerColours.Red:
                    return Color.red;
                case PlayerColours.Yellow:
                    return Color.yellow;
                case PlayerColours.Green:
                    return Color.green;
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }

        public void BeginSpawn()
        {
            StartCoroutine(BeginSpawnCoroutine());
        }
        
        

        IEnumerator BeginSpawnCoroutine(float delay = 0.4f)
        {
            foreach (PlayerConfig config in lobbyManager.playerConfigs)
            {
                yield return StartCoroutine(SpawnPlayerCoroutine(config));
                yield return new WaitForSeconds(delay);
            }

            
            // Data is still needed for results
            // Destroy(playerData.gameObject);
            playersJoined.Raise();
            yield return null;
        }

        IEnumerator SpawnPlayerCoroutine(PlayerConfig config)
        {
                if (!config.IsPlayer)
                    yield break;

                PlayerInput newPlayer = inputManager.JoinPlayer(config.playerIndex, config.splitScreenIndex,
                    config.controlScheme, config.deviceIds.Select(InputSystem.GetDeviceById).ToArray());

                // Update player character
                //Debug.Log("Spawn in player! charNo = " + config.characterNumber + ", charName = " + ((CharacterNames)config.characterNumber).ToString()
                //+ ". playNo = " + config.playerNumber + ", playColour = " + ((PlayerColours)config.playerNumber).ToString());
                newPlayer.GetComponent<Unit.Unit>().SetSpriteResolvers(
                    ((CharacterNames) config.characterNumber).ToString(),
                    ((PlayerColours) config.playerNumber).ToString());

                newPlayer.GetComponent<Unit.IUnit>().PlayerNumber = config.playerNumber;
                newPlayer.GetComponent<Unit.IUnit>().PlayerColour =
                    GetPlayerColour((PlayerColours) config.playerNumber);

                newPlayer.transform.position = spawnPoints[newPlayer.playerIndex].GetSpawnPosition();
                
                //Generate SpawnEffect on player spawn location
                GameObject spawnEffect =Instantiate(_spawnEffectPrefab, newPlayer.transform.position, Quaternion.identity);
                spawnEffect.GetComponent<AttachGameObjectsToParticles>().LightColour =
                    GetPlayerColour((PlayerColours) config.playerNumber);
                
             ParticleSystem.MainModule settings = spawnEffect.GetComponent<ParticleSystem>().main;
             settings.startColor = new ParticleSystem.MinMaxGradient(  GetPlayerColour((PlayerColours) config.playerNumber) );

        
            
                //Apply the appropriate material to use the dissolve effect
                UnitAnimator animator = newPlayer.gameObject.GetComponent<UnitAnimator>();
                

                Material dissolveMaterial  = Resources.Load<Material>($"Materials/Player{config.playerNumber}Dissolve");
                foreach (SpriteRenderer renderer in animator.Renderers)
                    renderer.material = dissolveMaterial;

                StartCoroutine(newPlayer.GetComponent<Unit.Unit>().RunDissolveCoroutine(dissolveMaterial));


                if (lobbyManager.CurrentGameMode == GameMode.TreasureHoard)
                {
                    _treasureHoardUI.GenerateTreasurePointUI(newPlayer, GetPlayerColour((PlayerColours) config.playerNumber));
                    GameObject reference = Instantiate(_playerTreasurePrefab, _playerTreasureSpawnPoint[config.playerNumber], Quaternion.identity);
                    reference.GetComponent<PlayerTreasure>().PlayerReference = newPlayer.GetComponent<IUnit>();
                    _treasures.Add( reference.GetComponent<PlayerTreasure>());
                }
     
                
                // Update inventory UI
                // Find inventory referenced by inventory controller contained by player prefab
                for (int i = 0; i < newPlayer.transform.childCount; i++)
                {
                    InventoryController temp = newPlayer.transform.GetChild(i)
                        .GetComponent<InventoryController>();

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
                    
                    
                    if (lobbyManager.CurrentGameMode == GameMode.TreasureHoard)
                    {
                        inv.inv[0] = _depositor;
                        inv.inv[1] = _detonator;
                    }
                }
                
                
                
        }

        public void SaveScores()
        {
            if (gameManager.Phase != GameManager.GamePhase.GameOver) return;

            if (lobbyManager.CurrentGameMode == GameMode.TreasureHoard)
            {
                int i = 0;
                
                foreach (var (playerConfig, inventoryController) in inventoryControllers)
                {
                    playerConfig.score = _treasures[i].CurrentPoints;
                    i++;
                }

                return;
            }
            
            
            foreach (var (playerConfig, inventoryController) in inventoryControllers)
            {
                playerConfig.score = inventoryController.Score;
            }

          
        }

        public  void GameEnded()
        {
            
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

        public void AllowAllInputs()
        {
            _allPlayers.ForEach(input =>
            {
                input.ActivateInput();
            });
        }

        // Message from Player Input Manager
        private void OnPlayerJoined(PlayerInput playerInput)
        {
            _allPlayers.Add(playerInput);
            playerInput.DeactivateInput();
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
    }
}