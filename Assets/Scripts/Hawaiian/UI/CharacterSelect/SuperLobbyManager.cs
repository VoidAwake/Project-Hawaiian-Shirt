using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hawaiian.Game;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.UI.CharacterSelect
{
    public class SuperLobbyManager : MonoBehaviour
    {
        [SerializeField] private SceneChanger sceneChanger;
        // TODO: UI should not be handling scene references
        [SerializeField] private SceneReference mainMenuScene;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private LobbyManager lobbyManager; // Character select
        [SerializeField] private ModeLobbyManager modeLobbyManager; // Mode select
        
        [SerializeField] private RectTransform transitionImage;                 // Scene reference. Covers screen when we transition from character to mode select.
        [SerializeField] private TextMeshProUGUI screenTitleTMP;
        // TODO: Initialise with this, should be in sync with the others
        [SerializeField] private PlayerConfigManager playerConfigManager;
        
        public bool isModeSelect = false; // Denotes what screen we're on
        
        public readonly List<LobbyPlayerController> lobbyPlayerControllers = new();
        
        private bool isExiting;
        
        
        private Coroutine transitionCoroutine;

        private void OnEnable()
        {
            lobbyManager.startGameRequested.AddListener(OnStartGameRequested);
            lobbyManager.mainMenuRequested.AddListener(OnMainMenuRequested);
            
            modeLobbyManager.mainMenuRequested.AddListener(OnModeMainMenuRequested);
            modeLobbyManager.startGameRequested.AddListener(OnModeStartGameRequested);
        }

        private void OnModeStartGameRequested()
        {
            StartGame();
        }

        private void OnModeMainMenuRequested()
        {
            if (transitionCoroutine == null)
                transitionCoroutine = StartCoroutine(TransitionToModeSelect(false));
        }

        private void OnDisable()
        {
            lobbyManager.startGameRequested.RemoveListener(OnStartGameRequested);
            lobbyManager.mainMenuRequested.RemoveListener(OnMainMenuRequested);
            
            modeLobbyManager.mainMenuRequested.RemoveListener(OnModeMainMenuRequested);
            modeLobbyManager.startGameRequested.RemoveListener(OnModeStartGameRequested);
        }

        private void Start()
        {
            lobbyManager.LobbyPlayerControllers = lobbyPlayerControllers;
            modeLobbyManager.LobbyPlayerControllers = lobbyPlayerControllers;
            ShowCharacterSelect(true);
            ShowModeSelect(false);
            transitionImage.gameObject.SetActive(false);
            screenTitleTMP.text = "Select Characters";
        }
        
        #region PlayerInputManager Messages

        private void OnPlayerJoined(PlayerInput playerInput)
        {
            // TODO: Disabled for now, test and see
            // Mode select stuff... dunno if this is a good idea, but
            // if (isModeSelect)
            // {
            //     Destroy(playerInput.gameObject);
            //     return;
            // }

            var playerConfig = playerConfigManager.AddPlayer(playerInput);
            
            var lobbyPlayerController = playerInput.GetComponent<LobbyPlayerController>();
            lobbyPlayerControllers.Add(lobbyPlayerController);
            lobbyPlayerController.Initialise(this, lobbyManager, modeLobbyManager, playerConfig);

            lobbyManager.JoinPlayer(playerConfig, lobbyPlayerController);
        }

        #endregion

        private void OnStartGameRequested()
        {
            // Mode select stuff... begin transition to the mode select screen
            if (transitionCoroutine == null)
                transitionCoroutine = StartCoroutine(TransitionToModeSelect(true));
        }

        private void OnMainMenuRequested()
        {
            if (isExiting) return;
            
            lobbyManager.GoingToMainMenu = true;
            sceneChanger.ChangeScene(mainMenuScene);
            isExiting = true;
        }
        
        public void StartGame()
        {
            if (!isExiting)
            {
                gameManager.CurrentGameMode = modeLobbyManager.CurrentGameMode;

                // Destroy(GetComponent<LobbyManager>());
                // Destroy(GetComponent<PlayerInputManager>());
                
                gameManager.LoadRandomLevel();
                
                isExiting = true;
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
        
        private void EnablePlayerControl(bool enable)
        {
            foreach (LobbyPlayerController player in lobbyManager.LobbyPlayerControllers)
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
        
        private void ShowCharacterSelect(bool enable)
        {
            lobbyManager.enabled = enable;
        }

        private void ShowModeSelect(bool enable)
        {
            modeLobbyManager.enabled = enable;
        }
        
        
        private void PromoteAllPlayerControllers(bool toGameModeSelect)
        {
            foreach (LobbyPlayerController player in lobbyManager.LobbyPlayerControllers)
            {
                if (player.Status == LobbyPlayerController.PlayerStatus.Ready && toGameModeSelect)
                    player.Status = LobbyPlayerController.PlayerStatus.SelectingMode;

                if (player.Status == LobbyPlayerController.PlayerStatus.SelectingMode && !toGameModeSelect)
                    player.Status = LobbyPlayerController.PlayerStatus.LoadedIn;
            }
        }
    }
}