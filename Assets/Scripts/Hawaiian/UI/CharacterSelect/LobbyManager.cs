using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Hawaiian.UI.CharacterSelect
{
    public class LobbyManager : MonoBehaviour
    {
        enum MenuState { CharacterSelect, MainMenu, BoardSelect, MinigameSelect }
        [SerializeField] MenuState menuState;
        [SerializeField] int subState;
        [SerializeField] int buildIndexOfNextScene;
        bool returningToMainMenu;
        LobbyGameManager LobbyGameManager;
        MenuState prevMenuState;
        bool nextReady;
        //bool backReady;

        //[SerializeField] Image transitionCheckers;
        //[SerializeField] Sprite[] transitionCheckersSprites;

        [Header("Character Select")]
        [SerializeField] LobbyWindow[] windows;
        [SerializeField] CanvasGroup[] portraits;

        [SerializeField] GameObject[] characterSelects;
        [SerializeField] Sprite[] characterSelectSprites;
        [SerializeField] TextMeshProUGUI readyText;
        [SerializeField] GameObject ready;

        public enum PlayerStatus
        {
            NotLoadedIn,
            LoadedIn,
            LoadedInAndSelected,
            SelectedComputerPlayer
        }

        public class LobbyPlayer
        {
            public PlayerStatus status;
            public LobbyWindow lobbyWindow;
            public int stickInput;
            public GameObject characterSelect;
            public PlayerConfig playerConfig;

            public LobbyPlayer(LobbyWindow lobbyWindow, GameObject characterSelect, PlayerConfig playerConfig)
            {
                this.lobbyWindow = lobbyWindow;
                this.characterSelect = characterSelect;
                this.playerConfig = playerConfig;
                
                status = PlayerStatus.NotLoadedIn;
                lobbyWindow.SetEmpty();
                characterSelect.gameObject.SetActive(false);
            }
        }

        public readonly LobbyPlayer[] lobbyPlayers = new LobbyPlayer[4];

        void Start()
        {
            LobbyGameManager = GetComponent<LobbyGameManager>();

            for (int i = 0; i < lobbyPlayers.Length; i++)
            {
                lobbyPlayers[i] = new LobbyPlayer(windows[i], characterSelects[i], LobbyGameManager.playerConfigs[i]);
            }

            ready.SetActive(false);
            //transitionCheckers.enabled = true;
            //transitionCheckers.sprite = transitionCheckersSprites[transitionCheckersSprites.Length - 1];
            //transitionTimer = 0.0f;
            subState = -1;
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(comPlayerNo);

            // Transition to next menu screen
            if (prevMenuState != menuState) HandleTransition();

            // Handle character select screen
            if (menuState == MenuState.CharacterSelect) HandleCharacterSelect();

            // Animate cursors
            foreach (var lobbyPlayer in lobbyPlayers)
            {
                if (lobbyPlayer.status == PlayerStatus.LoadedIn)
                {
                    // Animate cursor
                    lobbyPlayer.characterSelect.GetComponent<Image>().sprite = characterSelectSprites
                        [
                            Mathf.FloorToInt((Time.time % 0.39999f) * 10.0f) - (Mathf.FloorToInt((Time.time % 0.39999f) * 10.0f) > 2 ? 2 : 0)
                        ];
                }
                else
                {
                    // Set to still cursor
                    lobbyPlayer.characterSelect.GetComponent<Image>().sprite = characterSelectSprites[1];
                }
            }

            // Animate ready text
            if (nextReady)
            {
                readyText.color = new Color(readyText.color.r, readyText.color.g, readyText.color.b, 0.5f + 0.5f * Mathf.Sin(Time.time * 3.0f));
            }
        }

        void HandleTransition()
        {
            // Handle unloading previous state

            // Handle loading next state
            if (menuState == MenuState.CharacterSelect)
            {

            }

            // Set previous state to be current state
            prevMenuState = menuState;
        }

        void HandleCharacterSelect()
        {
            //if (subState == -1 || subState == 1)
            //{
            //    if (transitionTimer > 0.15f)
            //    {
            //        transitionTimer -= 0.15f;
            //        transitionInt++;

            //        if (transitionInt >= transitionCheckersSprites.Length)
            //        {
            //            // Transition out of this... transition.
            //            if (subState == -1)
            //            {
            //                transitionCheckers.enabled = false;
            //                transitionTimer = 0.0f;
            //                transitionInt = 0;
            //                subState++;
            //            }
            //            else
            //            {
            //                if (returningToMainMenu)
            //                {
            //                    SceneManager.LoadScene(0);
            //                }
            //                else
            //                {
            //                    // TODO: Grab player information from this class and LobbyGameManager (which has a PlayerConfig class) and pass it into the next scene

            //                    // Then, load the next scene
            //                    SceneManager.LoadScene(buildIndexOfNextScene);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            // Update the checkers' sprites
            //            if (subState == 1) transitionCheckers.sprite = transitionCheckersSprites[transitionInt];
            //            else transitionCheckers.sprite = transitionCheckersSprites[transitionCheckersSprites.Length - 1 - transitionInt];
            //        }
            //    }

            //    if (subState == -1 || subState == 1) transitionTimer += Time.deltaTime;
            //}
            if (subState != 0) return;
            
            // Handle loading in connected players
            foreach (PlayerConfig player in LobbyGameManager.playerConfigs)                              // For each player...
            {
                var lobbyPlayer = lobbyPlayers[player.playerNumber];
                if (lobbyPlayer.status == PlayerStatus.NotLoadedIn && player.IsPlayer)                                    // If they are a person and not yet loaded in, load them in.
                {
                    lobbyPlayer.lobbyWindow.SetUnselected();
                    lobbyPlayer.characterSelect.SetActive(true);
                    int count = 0;
                    int nextCharacter = player.characterNumber;
                    while (nextCharacter < 0)
                    {
                        if (CharacterNotChosen(count, true)) nextCharacter = count;
                        count++;
                    }
                    UpdateCharacterSelection(lobbyPlayer, nextCharacter);
                    lobbyPlayer.status = PlayerStatus.LoadedIn;
                    nextReady = false; ready.SetActive(false);
                    lobbyPlayer.stickInput = 0;
                }
            }

            // Handle player inputs
            foreach (var lobbyPlayer in lobbyPlayers)
            {
                switch (lobbyPlayer.status)
                {
                    case PlayerStatus.LoadedIn:
                        LoadedInPlayerInputHandling(lobbyPlayer);
                        break;
                }
            }
        }

        private void LoadedInPlayerInputHandling(LobbyPlayer lobbyPlayer)
        {
            if (lobbyPlayer.stickInput != 0) // Handle stick movement...
            {
                // Find the next available character to select
                int nextCharacter = (int)Mathf.Repeat(lobbyPlayer.playerConfig.characterNumber + lobbyPlayer.stickInput,
                    portraits.Length);
                while (!CharacterNotChosen(nextCharacter, true))
                {
                    nextCharacter = (int)Mathf.Repeat(nextCharacter + lobbyPlayer.stickInput, portraits.Length);
                }

                UpdateCharacterSelection(lobbyPlayer, nextCharacter);
                lobbyPlayer.stickInput = 0;
            }
        }

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

        bool AllPlayersSelected(bool allFour) // Use all four when checking that there are four players (human or not) with characters selected
        {
            int counter = 0;
            foreach (LobbyPlayer lobbyPlayer in lobbyPlayers)
            {
                if (lobbyPlayer.status == PlayerStatus.LoadedIn) return false;
                if (lobbyPlayer.status == PlayerStatus.NotLoadedIn)
                {
                    if (allFour) return false;
                    counter++;
                }
            }
            if (counter >= 4) return false;
            return true;
        }

        void ClearInputs()
        {
            foreach (var lobbyPlayer in lobbyPlayers)
            {
                lobbyPlayer.stickInput = 0;
            }
        }

        void UpdateCharacterSelection(LobbyPlayer lobbyPlayer, int charNumber)
        {
            PlayerConfig player = lobbyPlayer.playerConfig;
            lobbyPlayer.characterSelect.GetComponent<RectTransform>().anchoredPosition = new Vector2(portraits[charNumber].GetComponent<RectTransform>().anchoredPosition.x, portraits[charNumber].GetComponent<RectTransform>().anchoredPosition.y);
            player.characterNumber = charNumber;
            lobbyPlayer.lobbyWindow.UpdateHead(charNumber);
        }

        // TODO: Needs to be hooked up again
        public void UnloadOrReturnToMainMenu()
        {
            int counter = 0;
            int index = -1;
            foreach (LobbyPlayer lobbyPlayer in lobbyPlayers)
            {
                if (lobbyPlayer.status == PlayerStatus.NotLoadedIn) counter++;
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
            subState = 1;
            //returningToMainMenu = true;
            //transitionTimer = 0; transitionInt = 0; transitionCheckers.enabled = true;
            FindObjectOfType<Transition>().BeginTransition(true, true, 0, false);
        }

        public void IncrementSubState()
        {
            subState++;
        }

        public void OnPlayerActionA(int playerConfigPlayerNumber, InputValue value)
        {
            if (value.Get<float>() < 0.55f) return;
            
            var lobbyPlayer = lobbyPlayers[playerConfigPlayerNumber];

            if (lobbyPlayer.status == PlayerStatus.LoadedInAndSelected)
            {
                if (nextReady)
                {
                    if (AllPlayersSelected(false)) // Transition to next menu state
                    {
                        subState = 1; //transitionTimer = 0; transitionInt = 0; transitionCheckers.enabled = true;
                        FindObjectOfType<Transition>().BeginTransition(true, true, buildIndexOfNextScene, true);
                        Destroy(GetComponent<LobbyManager>());
                        Destroy(GetComponent<PlayerInputManager>());
                    }
            
                    ClearInputs();
                }
            }
            else if (lobbyPlayer.status == PlayerStatus.LoadedIn)
            {
                lobbyPlayer.lobbyWindow.SetSelected();
                portraits[lobbyPlayer.playerConfig.characterNumber].alpha = 0.2f;
                lobbyPlayer.status = PlayerStatus.LoadedInAndSelected;
                if (AllPlayersSelected(false))
                {
                    nextReady = true;
                    ready.SetActive(true);
                }
            }
        }

        public void OnPlayerActionB(int playerConfigPlayerNumber, InputValue value)
        {
            if (value.Get<float>() < 0.55f) return;
            
            var lobbyPlayer = lobbyPlayers[playerConfigPlayerNumber];

            if (lobbyPlayer.status == PlayerStatus.LoadedInAndSelected)
            {
                lobbyPlayer.lobbyWindow.SetUnselected();
                portraits[lobbyPlayer.playerConfig.characterNumber].alpha = 1.0f;
                lobbyPlayer.status = PlayerStatus.LoadedIn;
                nextReady = false;
                ready.SetActive(false);
                lobbyPlayer.stickInput = 0;
            }
            else if (lobbyPlayer.status == PlayerStatus.LoadedIn)
            {
                // Reset visuals for current player to unloaded in versions
                lobbyPlayer.lobbyWindow.SetEmpty();
                lobbyPlayer.characterSelect.SetActive(false);
                portraits[lobbyPlayer.playerConfig.characterNumber].alpha = 1.0f;

                // Unload player
                lobbyPlayer.status = PlayerStatus.NotLoadedIn;
                lobbyPlayer.playerConfig.manager.UnloadPlayer();

                if (AllPlayersSelected(false))
                {
                    nextReady = true;
                    ready.SetActive(true);
                }
            }
        }
    }
}
