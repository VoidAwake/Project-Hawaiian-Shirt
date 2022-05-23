using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Hawaiian.UI.CharacterSelect
{
    public class LobbyManager : MonoBehaviour
    {
        [SerializeField] int buildIndexOfNextScene;
        LobbyGameManager LobbyGameManager;
        bool nextReady;

        [Header("Character Select")]
        [SerializeField] LobbyWindow[] windows;
        [SerializeField] CanvasGroup[] portraits;

        [SerializeField] GameObject[] characterSelects;
        [SerializeField] Sprite[] characterSelectSprites;
        [SerializeField] TextMeshProUGUI readyText;
        [SerializeField] GameObject ready;

        private readonly List<LobbyPlayerController> lobbyPlayerControllers = new();

        #region MonoBehaviour Functions

        void Start()
        {
            LobbyGameManager = GetComponent<LobbyGameManager>();

            ready.SetActive(false);
        }

        void Update()
        {
            // Animate cursors
            foreach (var lobbyPlayerController in lobbyPlayerControllers)
            {
                if (lobbyPlayerController.status == LobbyPlayerController.PlayerStatus.LoadedIn)
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
            if (nextReady)
            {
                readyText.color = new Color(readyText.color.r, readyText.color.g, readyText.color.b, 0.5f + 0.5f * Mathf.Sin(Time.time * 3.0f));
            }
        }

        #endregion

        #region PlayerInputManager Messages

        private void OnPlayerJoined(PlayerInput playerInput)
        {
            var playerConfig = LobbyGameManager.AddPlayer(playerInput);
            
            var lobbyPlayerController = playerInput.GetComponent<LobbyPlayerController>();
            lobbyPlayerControllers.Add(lobbyPlayerController);
            lobbyPlayerController.Initialise(this, windows[playerConfig.playerNumber], characterSelects[playerConfig.playerNumber], playerConfig);
        }

        #endregion
        
        bool CharacterNotChosen(int characterNumber, bool onlyPlayers) // SOMEWHERE, WHEN TRANSITIONING/SETTING UP CHARACTER SELECT, YOU MUST SET characterNumber TO -1 FOR EACH playerConfig WHO !isPlayer // Is this still true?
        {
            foreach (PlayerConfig playerConfig in LobbyGameManager.playerConfigs)
            {
                if (!onlyPlayers || playerConfig.IsPlayer) // If only checking human players, and player is not a human, then don't check
                {
                    if (playerConfig.characterNumber == characterNumber) return false;
                }
            }
            return true;
        }

        bool AllPlayersReady(bool allFour) // Use all four when checking that there are four players (human or not) with characters selected
        {
            if (lobbyPlayerControllers.Count == 0) return false;
            
            int counter = 0;
            foreach (LobbyPlayerController lobbyPlayerController in lobbyPlayerControllers)
            {
                if (lobbyPlayerController.status == LobbyPlayerController.PlayerStatus.LoadedIn) return false;
                if (lobbyPlayerController.status == LobbyPlayerController.PlayerStatus.NotLoadedIn)
                {
                    if (allFour) return false;
                    counter++;
                }
            }
            if (counter >= 4) return false;
            return true;
        }

        // TODO: Needs to be hooked up again
        public void UnloadOrReturnToMainMenu()
        {
            int counter = 0;
            int index = -1;
            foreach (LobbyPlayerController lobbyPlayerController in lobbyPlayerControllers)
            {
                if (lobbyPlayerController.status == LobbyPlayerController.PlayerStatus.NotLoadedIn) counter++;
                // else index = i;
            }

            if (counter < 3) return;
            
            // TODO: Commented, because I can't work out what this code is meant to do
            // if (index >= 0)
            // {
            //     // Reset visuals for current player to unloaded in versions
            //     windows[index].SetEmpty();
            //     characterSelects[index].SetActive(false);
            //     portraits[LobbyGameManager.playerConfigs[index].characterNumber].alpha = 1.0f;
            //
            //     // Unload player
            //     playerStatuses[index] = 0;
            //     LobbyGameManager.playerConfigs[index].manager.UnloadPlayer();
            //     playerButtonInputs[index] = 0;
            // }

            // Exit this scene
            FindObjectOfType<Transition>().BeginTransition(true, true, 0, false);
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
            if (!nextReady) return;

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
            
            nextReady = readyBool;
            
            ready.SetActive(readyBool);
        }

        public void UnloadPlayer(LobbyPlayerController lobbyPlayerController)
        {
            Destroy(lobbyPlayerController.gameObject);

            lobbyPlayerControllers.Remove(lobbyPlayerController);
            
            UpdateReadyToStart();
        }
    }
}
