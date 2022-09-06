using System.Collections.Generic;
using System.Linq;
using Hawaiian.Inventory;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Game.GameModes
{
    public abstract class ModeManager : ScriptableObject
    {
        [Header("Mode Manager")]
        [SerializeField] private string displayName;
        [SerializeField] private string description;
        [SerializeField] private GameObject controlsInstructionsDialoguePrefab;
        [SerializeField] private GameObject tutorialDialoguePrefab;
        [SerializeField] protected SceneChanger sceneChanger;
        [SerializeField] protected GameEvent gameOverEvent;

        public static ModeManager CurrentModeManager { get; set; }
        public static UnityEvent<ModeManager> Initialised { get; } = new();
        public UnityEvent winningPlayersChanged = new();
        
        public virtual GameObject ControlsInstructionsDialoguePrefab => controlsInstructionsDialoguePrefab;
        public virtual GameObject TutorialDialoguePrefab => tutorialDialoguePrefab;
        public virtual string DisplayName => displayName;
        public virtual string Description => description;

        public UnityEvent<PlayerConfig> playerJoined = new();
        protected UnityEvent gameOver = new();

        protected PlayerManager playerManager;
        protected Dictionary<PlayerConfig, InventoryController> inventoryControllers = new();
        
        public virtual void LoadRandomLevel() { }
        public virtual void SaveScores() { }

        protected virtual void OnPlayerJoined(PlayerConfig playerConfig)
        {
            // TODO: Duplicate code. See GameDialogue.OnPlayerJoined.
            var inventoryController = playerConfig.playerInput.GetComponentInChildren<InventoryController>();
            
            inventoryControllers.Add(playerConfig, inventoryController);

            // TODO: Remove listener
            // TODO: Are we sure this is the right event?
            inventoryController.inv.currentItemChanged.AddListener(UpdateWinningPlayers);
            
            playerJoined.Invoke(playerConfig);
        }

        private void OnEnable()
        {
            gameOver.AddListener(OnGameOver);
        }

        private void OnDisable()
        {
            gameOver.RemoveListener(OnGameOver);
        }

        private void OnGameOver()
        {
            SaveScores();
        }
        
        public List<Transform> WinningPlayers { get; private set; }

        // TODO: Hand off this logic to the specific mode managers
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

    }
}