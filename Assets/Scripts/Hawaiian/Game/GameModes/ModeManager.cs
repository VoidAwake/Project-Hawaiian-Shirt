using System.Collections.Generic;
using System.Linq;
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
        // TODO: Never invoked?
        public static UnityEvent<ModeManager> Initialised { get; } = new();
        public UnityEvent winningPlayersChanged = new();
        
        public virtual GameObject ControlsInstructionsDialoguePrefab => controlsInstructionsDialoguePrefab;
        public virtual GameObject TutorialDialoguePrefab => tutorialDialoguePrefab;
        public virtual string DisplayName => displayName;
        public virtual string Description => description;

        public UnityEvent<PlayerConfig> playerJoined = new();
        protected UnityEvent gameOver = new();

        protected PlayerManager playerManager;
        protected readonly List<PlayerConfig> playerConfigs = new();
        
        public virtual void LoadRandomLevel() { }
        
        public virtual void SaveScores()
        {
            foreach (var playerConfig in playerConfigs)
            {
                playerConfig.score = PlayerConfigScore(playerConfig);
            }
        }

        protected virtual void OnPlayerJoined(PlayerConfig playerConfig)
        {
            playerConfigs.Add(playerConfig);
            
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

        // TODO: Could cache results for better performance
        protected void UpdateWinningPlayers()
        {
            var playerConfigScores = new Dictionary<PlayerConfig, float>();

            var maxScore = 0f;

            foreach (var playerConfig in playerConfigs)
            {
                var score = PlayerConfigScore(playerConfig);

                playerConfigScores.Add(playerConfig, score);

                if (maxScore < score)
                    maxScore = score;
            }

            WinningPlayers = playerConfigScores
                .Where(o => o.Value == maxScore)
                .Select(o => o.Key.playerInput.transform)
                .ToList();
            
            if (WinningPlayers.Count == playerConfigs.Count)
                WinningPlayers.Clear();
            
            winningPlayersChanged.Invoke();
        }

        protected virtual float PlayerConfigScore(PlayerConfig playerConfig)
        {
            return 0;
        }

        public void SetModeAsCurrent()
        {
            CurrentModeManager = this;
        }

        public virtual void ListenToLevel()
        {
            // TODO: Come back to this
            playerManager = FindObjectOfType<PlayerManager>();

            // TODO: Unlisten?
            playerManager.playerJoined.AddListener(OnPlayerJoined);
            
            // TODO: Unlisten?
            gameOverEvent.RegisterListener(gameOver);
        }
    }
}