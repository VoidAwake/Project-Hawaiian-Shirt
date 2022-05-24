using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Hawaiian.UI.CharacterSelect
{
    public class LobbyManager : MonoBehaviour
    {
        [SerializeField] private int buildIndexOfNextScene;

        [Header("Character Select")]
        [SerializeField] private LobbyWindow[] windows;
        [SerializeField] private CanvasGroup[] portraits;
        [SerializeField] private GameObject[] characterSelects;
        [SerializeField] private Sprite[] characterSelectSprites;
        [SerializeField] private TextMeshProUGUI readyText;
        [SerializeField] private GameObject ready;

        private readonly List<LobbyPlayerController> lobbyPlayerControllers = new();
        private LobbyGameManager lobbyGameManager;
        private bool readyToStartGame;

        #region MonoBehaviour Functions

        private void Start()
        {
            lobbyGameManager = GetComponent<LobbyGameManager>();

            ready.SetActive(false);
        }

        private void Update()
        {
            // Animate cursors
            foreach (var lobbyPlayerController in lobbyPlayerControllers)
            {
                if (lobbyPlayerController.Status == LobbyPlayerController.PlayerStatus.LoadedIn)
                {
                    // Animate cursor
                    lobbyPlayerController.characterSelect.GetComponent<Image>().sprite = characterSelectSprites
                    [
                        Mathf.FloorToInt((Time.time % 0.39999f) * 10.0f) - (Mathf.FloorToInt((Time.time % 0.39999f) * 10.0f) > 2 ? 2 : 0)
                    ];
                }
                else
                {
                    // Set to still cursor
                    lobbyPlayerController.characterSelect.GetComponent<Image>().sprite = characterSelectSprites[1];
                }
            }

            // Animate ready text
            if (readyToStartGame)
            {
                readyText.color = new Color(readyText.color.r, readyText.color.g, readyText.color.b, 0.5f + 0.5f * Mathf.Sin(Time.time * 3.0f));
            }
        }

        #endregion

        #region PlayerInputManager Messages

        private void OnPlayerJoined(PlayerInput playerInput)
        {
            var playerConfig = lobbyGameManager.AddPlayer(playerInput);
            
            var lobbyPlayerController = playerInput.GetComponent<LobbyPlayerController>();
            lobbyPlayerControllers.Add(lobbyPlayerController);
            lobbyPlayerController.Initialise(this, windows[playerConfig.playerNumber], characterSelects[playerConfig.playerNumber], playerConfig);
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

        private bool AllPlayersReady(bool allFour) // Use all four when checking that there are four players (human or not) with characters selected
        {
            if (lobbyPlayerControllers.Count == 0) return false;
            
            int counter = 0;
            foreach (LobbyPlayerController lobbyPlayerController in lobbyPlayerControllers)
            {
                if (lobbyPlayerController.Status == LobbyPlayerController.PlayerStatus.LoadedIn) return false;
                if (lobbyPlayerController.Status == LobbyPlayerController.PlayerStatus.NotLoadedIn)
                {
                    if (allFour) return false;
                    counter++;
                }
            }
            if (counter >= 4) return false;
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
            if (!readyToStartGame) return;

            FindObjectOfType<Transition>().BeginTransition(true, true, buildIndexOfNextScene, true);
            Destroy(GetComponent<LobbyManager>());
            Destroy(GetComponent<PlayerInputManager>());
        }

        public CanvasGroup GetPortrait(int characterNumber)
        {
            return portraits[characterNumber];
        }

        public void UpdateReadyToStart()
        {
            var readyBool = AllPlayersReady(false);
            
            readyToStartGame = readyBool;
            
            ready.SetActive(readyBool);
        }
        
        public void RequestMainMenu()
        {
            // TODO: Update this condition
            if (lobbyPlayerControllers.Count > -1) return;
            
            FindObjectOfType<Transition>().BeginTransition(true, true, -1, false);
        }
    }
}
