using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Hawaiian.UI.CharacterSelect
{
    public class LobbyManager : MonoBehaviour
    {
        [SerializeField] private int buildIndexOfNextScene;

        [Header("Character Select")]
        [SerializeField] private LobbyWindow[] windows;
        [SerializeField] private CanvasGroup[] portraits;
        [SerializeField] private CharacterPortraitCursor[] portraitCursors;

        public UnityEvent readyChanged = new();

        public bool ReadyToStartGame
        {
            get => readyToStartGame;
            private set
            {
                readyToStartGame = value;
                readyChanged.Invoke();
            }
        }

        private readonly List<LobbyPlayerController> lobbyPlayerControllers = new();
        private LobbyGameManager lobbyGameManager;
        private bool readyToStartGame;
        private bool isExiting;

        #region MonoBehaviour Functions

        private void Start()
        {
            lobbyGameManager = GetComponent<LobbyGameManager>();
        }

        #endregion

        #region PlayerInputManager Messages

        private void OnPlayerJoined(PlayerInput playerInput)
        {
            var playerConfig = lobbyGameManager.AddPlayer(playerInput);
            
            var lobbyPlayerController = playerInput.GetComponent<LobbyPlayerController>();
            lobbyPlayerControllers.Add(lobbyPlayerController);
            lobbyPlayerController.Initialise(this, playerConfig);
            
            windows[playerConfig.playerNumber].Initialise(lobbyPlayerController);
            portraitCursors[playerConfig.playerNumber].Initialise(lobbyPlayerController);
        }

        #endregion

        private bool CharacterNotChosen(int characterNumber, bool onlyPlayers) // SOMEWHERE, WHEN TRANSITIONING/SETTING UP CHARACTER SELECT, YOU MUST SET characterNumber TO -1 FOR EACH playerConfig WHO !isPlayer // Is this still true?
        {
            foreach (var lobbyPlayerController in lobbyPlayerControllers)
            {
                if (lobbyPlayerController.Status == LobbyPlayerController.PlayerStatus.NotLoadedIn) continue;
                
                if (!onlyPlayers || lobbyPlayerController.playerConfig.IsPlayer) // If only checking human players, and player is not a human, then don't check
                {
                    if (lobbyPlayerController.playerConfig.characterNumber == characterNumber) return false;
                }
            }
            return true;
        }

        public int FirstUnselectedCharacter()
        {
            return FirstUnselectedCharacterFrom(-1, 1);
        }

        public int FirstUnselectedCharacterFrom(int startIndex, int direction)
        {
            int index = (int)Mathf.Repeat(startIndex + direction, portraits.Length);
            
            while (!CharacterNotChosen(index, true) && index != startIndex)
            {
                index = (int)Mathf.Repeat(index + direction, portraits.Length);
            }

            return index;
        }

        public void RequestStartGame()
        {
            if (!ReadyToStartGame) return;

            if (!isExiting)
            {
                FindObjectOfType<Transition>().BeginTransition(true, true, buildIndexOfNextScene);
                Destroy(GetComponent<LobbyManager>());
                Destroy(GetComponent<PlayerInputManager>());
                isExiting = true;
            }
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
            if (lobbyPlayerControllers.Count == 0) return false;

            return lobbyPlayerControllers.All(l => l.Status != LobbyPlayerController.PlayerStatus.LoadedIn);
        }

        private bool AllPlayersHaveStatus(LobbyPlayerController.PlayerStatus playerStatus)
        {
            return lobbyPlayerControllers.All(l => l.Status == playerStatus);
        }

        // TODO: Make the buildIndex a variable.
        public bool RequestMainMenu()
        {
            if (!isExiting)
            {
                if (!AllPlayersHaveStatus(LobbyPlayerController.PlayerStatus.NotLoadedIn))
                    return false;

                FindObjectOfType<Transition>().BeginTransition(true, true, 0);
                isExiting = true;

                return true;
            }
            return false;
        }
    }
}
