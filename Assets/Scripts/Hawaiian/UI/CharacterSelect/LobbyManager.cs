using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hawaiian.Game;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Hawaiian.UI.MainMenu;
using TMPro;
using UnityEngine.UI;

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
        [SerializeField] private SceneChanger sceneChanger;
        // TODO: UI should not be handling scene references
        [SerializeField] private SceneReference mainMenuScene;
        [SerializeField] private GameManager gameManager;

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
        private bool readyToStartGame;
        private bool isExiting;

        // Mode select stuff... new properties
        [Serializable] private struct GameModeString { public GameMode gameMode; public string text; }
        [Serializable] private struct GameModeInt { public GameMode gameMode; public int number; }

        [Header("Mode Select")]
        [SerializeField] private GameObject characterSelectCanvas;
        [SerializeField] private GameObject modeSelectCanvas;
        [SerializeField] private GameMode[] gameModes;
        [SerializeField] private List<GameModeString> gameModeNames;
        [SerializeField] private List<GameModeString> gameModeDescriptions;
        [SerializeField] private List<GameModeInt> gameModeBuildIndex;
        [SerializeField] private RectTransform transitionImage;                 // Scene reference. Covers screen when we transition from character to mode select.
        [SerializeField] private Image[] selectedPortraits;                     // Scene reference. Update sprites (and positions) to match player's characters.
        [SerializeField] private Image[] selectedBGs;                           // Scene reference. Reduce alpha on BG of unselected characters.
        [SerializeField] private Sprite[] portraitSprites;                      // Asset reference. Sprites for character portraits.
        [SerializeField] private Vector2[] selectedPortraitOffsets;             // To be entered in inspector. Individual offsets for each character's portrait sprite.
        [SerializeField] private GameObject buttonPrefab;                       // Asset reference. To be instantiated in runtime, set up, and assigned to menu controller.
        [SerializeField] private GridLayoutGroup buttonsParent;                 // Scene reference. To be positioned, and have buttons parented to.
        [SerializeField] private TextMeshProUGUI gameModeNameTMP;
        [SerializeField] private TextMeshProUGUI gameModeDescriptionTMP;
        [SerializeField] private TextMeshProUGUI screenTitleTMP;
        public MainMenuController menuController;                               // Scene reference. To assign button refernce to, and redirect player inputs.
        public bool isModeSelect = false; // Denotes what screen we're on
        private Coroutine transitionCoroutine;
        
        public GameMode CurrentGameMode;
        
        #region MonoBehaviour Functions

        private void Start()
        {
            // Mode select stuff...
            CreateGameModeButtons();
            ShowCharacterSelect(true);
            ShowModeSelect(false);
            transitionImage.gameObject.SetActive(false);
            screenTitleTMP.text = "Select Characters";
            
            playerConfigManager.Clear();
        }
        
        #endregion

        #region PlayerInputManager Messages

        private void OnPlayerJoined(PlayerInput playerInput)
        {
            // Mode select stuff... dunno if this is a good idea, but
            if (isModeSelect)
            {
                Destroy(playerInput.gameObject);
                return;
            }

            var playerConfig = playerConfigManager.AddPlayer(playerInput);
            
            var lobbyPlayerController = playerInput.GetComponent<LobbyPlayerController>();
            lobbyPlayerControllers.Add(lobbyPlayerController);
            lobbyPlayerController.Initialise(this, playerConfig);
            
            windows[playerConfig.playerNumber].Initialise(lobbyPlayerController);
            portraitCursors[playerConfig.playerNumber].Initialise(lobbyPlayerController);
        }

        #endregion

        private bool CharacterChosen(int characterNumber, bool onlyPlayers) // SOMEWHERE, WHEN TRANSITIONING/SETTING UP CHARACTER SELECT, YOU MUST SET characterNumber TO -1 FOR EACH playerConfig WHO !isPlayer // Is this still true?
        {
            return lobbyPlayerControllers
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

            // Mode select stuff... begin transition to the mode select screen
            if (transitionCoroutine == null)
                transitionCoroutine = StartCoroutine(TransitionToModeSelect(true));
        }

        public void StartGame()
        {
            if (!isExiting)
            {
                gameManager.CurrentGameMode = CurrentGameMode;

                // Destroy(GetComponent<LobbyManager>());
                // Destroy(GetComponent<PlayerInputManager>());
                
                gameManager.LoadRandomLevel();
                
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
            // if (lobbyPlayerControllers.Count == 0) return false; // No longer returns false, because these are now hidden, not destroyed
            if (AllPlayersHaveInvalidCharacters())
                return false;

            return lobbyPlayerControllers.All(l => l.Status != LobbyPlayerController.PlayerStatus.LoadedIn);
        }

        private bool AllPlayersHaveStatus(LobbyPlayerController.PlayerStatus playerStatus)
        {
            return lobbyPlayerControllers.All(l => l.Status == playerStatus);
        }

        public bool RequestMainMenu()
        {
            if (!isExiting)
            {
                if (!AllPlayersHaveStatus(LobbyPlayerController.PlayerStatus.NotLoadedIn))
                    return false;

                sceneChanger.ChangeScene(mainMenuScene);
                isExiting = true;

                return true;
            }
            return false;
        }

        #region Ruining Jack's Refactored Code (Mode Select Functions)
        private void CreateGameModeButtons()
        {
            // Create buttons for mode select screen
            Button[] buttons = new Button[gameModes.Length];
            int counter = 0;
            foreach (GameMode gameMode in gameModes)
            {
                GameObject button = Instantiate(buttonPrefab, buttonsParent.transform);                                             // Create and parent button
                float yPos = counter * -130.0f;
                button.transform.localPosition = new Vector2(0.0f, yPos);
                if (counter == 0) menuController.cursor.transform.localPosition = button.transform.localPosition;                   // Set cursor to initial position, for first element
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GetGameModeString(gameMode, gameModeNames);     // Set button text to game mode name
                MainMenuButtonFunctions buttonFunction = button.gameObject.AddComponent<MainMenuButtonFunctions>();                 // Add button function component
                buttonFunction.SetButtonFunction(0, GetGameModeInt(gameMode, gameModeBuildIndex), true,gameMode);                            // Set button's function to load build index associated with game mode
                buttonFunction.isModeSelectButton = true;                                                                           // Set button to tell this script to update selection's decription
                buttons[counter] = button.GetComponent<Button>();
                counter++;
            }
            menuController.buttons = buttons;                                                                                       // Assign reference to new buttons in menu controller
            buttonsParent.transform.localPosition = new Vector2(-200.0f,
                ((gameModes.Length * 100.0f) + ((gameModes.Length - 1) * 30.0f)) / 2.0f - 50.0f);                                   // Position buttons' parent so that they're centred on screen
            menuController.cursor.transform.parent.localPosition = new Vector2(-200.0f,
                ((gameModes.Length * 100.0f) + ((gameModes.Length - 1) * 30.0f)) / 2.0f - 50.0f);                                   // Gotta move parent of cursor too, because awesome code

            // Update textmesh elements
            UpdateGameModeDescription(0);
        }

        private string GetGameModeString(GameMode toFind, List<GameModeString> list)
        {
            foreach (GameModeString element in list)
                if (element.gameMode == toFind) return element.text;

            return toFind.ToString();
        }

        private int GetGameModeInt(GameMode toFind, List<GameModeInt> list)
        {
            foreach (GameModeInt element in list)
                if (element.gameMode == toFind) return element.number;

            return -1;
        }

        private void PromoteAllPlayerControllers(bool toGameModeSelect)
        {
            foreach (LobbyPlayerController player in lobbyPlayerControllers)
            {
                if (player.Status == LobbyPlayerController.PlayerStatus.Ready && toGameModeSelect)
                    player.Status = LobbyPlayerController.PlayerStatus.SelectingMode;

                if (player.Status == LobbyPlayerController.PlayerStatus.SelectingMode && !toGameModeSelect)
                    player.Status = LobbyPlayerController.PlayerStatus.LoadedIn;
            }
        }

        private void ShowCharacterSelect(bool enable)
        {
            characterSelectCanvas.gameObject.SetActive(enable);
        }

        private void ShowModeSelect(bool enable)
        {
            modeSelectCanvas.gameObject.SetActive(enable);
            menuController.disabled = !enable;

            // Format buttons
            //buttonsParent.enabled = !enable;
            //menuController.cursor.gameObject.SetActive(enable);


            // Also set up chosen character portraits here, if enable is true
            if (enable)
                SetUpSelectedCharacterUI();
        }

        private void SetUpSelectedCharacterUI()
        {
            for (int i = 0; i < 4; i++)
            {
                bool isSelected = playerConfigManager.playerConfigs[i].characterNumber >= 0;
                selectedBGs[i].color = new Color(selectedBGs[i].color.r, selectedBGs[i].color.g, selectedBGs[i].color.b, isSelected ? 1.0f : 0.15f);        // Set alpha of portrait BG
                selectedPortraits[i].enabled = isSelected;                                                                                                  // Enable sprite or not
                if (isSelected) selectedPortraits[i].sprite = portraitSprites[playerConfigManager.playerConfigs[i].characterNumber];                           // Set image sprite
            }
        }

        private void EnablePlayerControl(bool enable)
        {
            foreach (LobbyPlayerController player in lobbyPlayerControllers)
            {
                if (player.Status != LobbyPlayerController.PlayerStatus.NotLoadedIn)
                    player.inputEnabled = enable;
            }
        }

        private IEnumerator ReplaceScreenTitleText(string newText)
        {
            // Animate deleting current title
            while (screenTitleTMP.text.Length > 0)
            {
                screenTitleTMP.text = screenTitleTMP.text.Remove(screenTitleTMP.text.Length - 1);
                yield return new WaitForSeconds(0.01f);
            }

            // Animate typing up new title
            while (screenTitleTMP.text.Length < newText.Length)
            {
                screenTitleTMP.text = screenTitleTMP.text.Insert(screenTitleTMP.text.Length, newText.ElementAt(screenTitleTMP.text.Length).ToString());
                yield return new WaitForSeconds(0.01f);
            }
        }

        private IEnumerator TransitionToModeSelect(bool toGameModeSelect)
        {
            float direction = toGameModeSelect ? 1.0f : -1.0f;
            bool reachedMidPoint = false;
            float speed = 10000.0f;
            int fudgeCounter = 0;

            // Replace title text at top of screen
            StartCoroutine(ReplaceScreenTitleText(toGameModeSelect ? "Select a Game Mode" : "Select Characters"));

            // Set up image, and other important stuff(s)
            PromoteAllPlayerControllers(toGameModeSelect);
            EnablePlayerControl(false);
            transitionImage.gameObject.SetActive(true);
            transitionImage.localPosition = new Vector2(direction * 1920.0f, 0.0f); // We better, uh, fix the screen dimensions to 16:9
            isModeSelect = toGameModeSelect;

            // Animate image, over time (the 1st part)
            while (!reachedMidPoint)
            {
                transitionImage.localPosition = new Vector2(transitionImage.localPosition.x - direction * Time.deltaTime * speed, 0.0f);

                if (fudgeCounter < 5)
                {
                    fudgeCounter++;
                    EnablePlayerControl(false); // Tryna fight against the coroutine in lobby player controller that reenables inputs when player status changes
                }

                if (transitionImage.localPosition.x * direction < 0.0f)
                {
                    transitionImage.localPosition = new Vector2(0.0f, 0.0f);
                    reachedMidPoint = true;
                }

                yield return new WaitForEndOfFrame();
            }

            // Do something, when screen is totally hidden
            ShowCharacterSelect(!toGameModeSelect);
            ShowModeSelect(toGameModeSelect);
            yield return new WaitForSeconds(0.2f);

            // Animate image, over time (the 2nd part)
            while (reachedMidPoint)
            {
                transitionImage.localPosition = new Vector2(transitionImage.localPosition.x - direction * Time.deltaTime * speed, 0.0f);

                if (transitionImage.localPosition.x * direction < -1920.0f)
                {
                    transitionImage.localPosition = new Vector2(-1920.0f * direction, 0.0f);
                    reachedMidPoint = false;
                }

                yield return new WaitForEndOfFrame();
            }

            // Finish animation and reenable all neccesary thing-o's
            transitionImage.gameObject.SetActive(false);
            EnablePlayerControl(true);
            transitionCoroutine = null;
        }

        public void TryExitModeSelect()
        {
            if (transitionCoroutine == null)
                transitionCoroutine = StartCoroutine(TransitionToModeSelect(false));
        }

        public void UpdateGameModeDescription(int index)
        {
            gameModeNameTMP.text = GetGameModeString(gameModes[index], gameModeNames);
            gameModeDescriptionTMP.text = GetGameModeString(gameModes[index], gameModeDescriptions);
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
        #endregion 
    }
}
