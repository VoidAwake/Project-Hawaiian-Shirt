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
        
        public virtual GameObject ControlsInstructionsDialoguePrefab => controlsInstructionsDialoguePrefab;
        public virtual GameObject TutorialDialoguePrefab => tutorialDialoguePrefab;
        public virtual string DisplayName => displayName;
        public virtual string Description => description;

        public UnityEvent<PlayerConfig> playerJoined = new();
        protected UnityEvent gameOver = new();

        protected PlayerManager playerManager;
        
        public virtual void LoadRandomLevel() { }
        public virtual void SaveScores() { }

        protected virtual void OnPlayerJoined(PlayerConfig playerConfig)
        {
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

        public void SetModeAsCurrent()
        {
            CurrentModeManager = this;
        }

        public void ListenToLevel()
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