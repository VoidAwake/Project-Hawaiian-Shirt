using System.Collections.Generic;
using System.Linq;
using Hawaiian.Game;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Hawaiian.UI.CharacterSelect
{
    public class LobbyManager : MonoBehaviour
    {
        [Header("Character Select")]
        // TODO: Separate the responsibilities of lobby management and UI
        [SerializeField] private LobbyWindow[] windows;
        [SerializeField] private CanvasGroup[] portraits;
        [SerializeField] private CharacterPortraitCursor[] portraitCursors;
        [SerializeField] private PlayerConfigManager playerConfigManager;
        [SerializeField] private GameObject characterSelectCanvas;

        public UnityEvent readyChanged = new();
        public UnityEvent startGameRequested = new();
        public UnityEvent mainMenuRequested = new();

        public bool ReadyToStartGame
        {
            get => readyToStartGame;
            private set
            {
                readyToStartGame = value;
                readyChanged.Invoke();
            }
        }

        public bool GoingToMainMenu { get; set; }

        private bool readyToStartGame;

        public List<LobbyPlayerController> LobbyPlayerControllers { get; set; }

        #region MonoBehaviour Functions

        private void OnEnable()
        {
            if (characterSelectCanvas) characterSelectCanvas.SetActive(true);
        }
        
        private void OnDisable()
        {
            if (characterSelectCanvas) characterSelectCanvas.SetActive(false);
        }

        private void Start()
        {
            playerConfigManager.Clear();
        }
        
        #endregion

        private bool CharacterChosen(int characterNumber, bool onlyPlayers) // SOMEWHERE, WHEN TRANSITIONING/SETTING UP CHARACTER SELECT, YOU MUST SET characterNumber TO -1 FOR EACH playerConfig WHO !isPlayer // Is this still true?
        {
            return LobbyPlayerControllers
                .Where(lobbyPlayerController => lobbyPlayerController.Status != LobbyPlayerController.PlayerStatus.NotLoadedIn)
                .Where(lobbyPlayerController => !onlyPlayers || lobbyPlayerController.playerConfig.IsPlayer)
                .Any(lobbyPlayerController => lobbyPlayerController.playerConfig.characterNumber == characterNumber);
        }

        public int FirstUnselectedCharacter()
        {
            return FirstUnselectedCharacterFrom(-1, 1);
        }

        public int FirstUnselectedCharacterFrom(int startIndex, int direction)
        {
            int index = startIndex;

            do index = (int) Mathf.Repeat(index + direction, portraits.Length);
            
            while (CharacterChosen(index, true) && index != startIndex);

            return index;
        }

        public void RequestStartGame() // This now initiates progressing from character select to game select
        {
            if (!ReadyToStartGame) return;
            
            startGameRequested.Invoke();
        }

        public CanvasGroup GetPortrait(int characterNumber)
        {
            return portraits[characterNumber];
        }

        public void UpdateReadyToStart()
        {
            ReadyToStartGame = AllPlayersReady();
        }

        private bool AllPlayersReady()
        {
            // if (lobbyPlayerControllers.Count == 0) return false; // No longer returns false, because these are now hidden, not destroyed
            if (AllPlayersHaveInvalidCharacters())
                return false;

            return LobbyPlayerControllers.All(l => l.Status != LobbyPlayerController.PlayerStatus.LoadedIn);
        }

        private bool AllPlayersHaveStatus(LobbyPlayerController.PlayerStatus playerStatus)
        {
            return LobbyPlayerControllers.All(l => l.Status == playerStatus);
        }

        public void RequestMainMenu()
        {
            if (!AllPlayersHaveStatus(LobbyPlayerController.PlayerStatus.NotLoadedIn)) return;
            
            mainMenuRequested.Invoke();
        }
        
        private bool AllPlayersHaveInvalidCharacters()
        {
            foreach (PlayerConfig config in playerConfigManager.playerConfigs)
            {
                if (config.characterNumber >= 0)
                    return false;
            }

            return true;
        }
        
        public void JoinPlayer(PlayerConfig playerConfig, LobbyPlayerController lobbyPlayerController)
        {
            windows[playerConfig.playerNumber].Initialise(lobbyPlayerController);
            portraitCursors[playerConfig.playerNumber].Initialise(lobbyPlayerController);
        }
    }
}
