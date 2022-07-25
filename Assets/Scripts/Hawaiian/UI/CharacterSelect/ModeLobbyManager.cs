using System;
using System.Collections.Generic;
using Hawaiian.Game;
using Hawaiian.Game.GameModes;
using Hawaiian.UI.General;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Hawaiian.UI.CharacterSelect
{
    public class ModeLobbyManager : MonoBehaviour
    {
        [Header("Mode Select")]
        [SerializeField] private GameModeSO[] gameModeSOs;
        [SerializeField] private Image[] selectedPortraits;                     // Scene reference. Update sprites (and positions) to match player's characters.
        [SerializeField] private Image[] selectedBGs;                           // Scene reference. Reduce alpha on BG of unselected characters.
        [SerializeField] private Sprite[] portraitSprites;                      // Asset reference. Sprites for character portraits.
        [SerializeField] private Vector2[] selectedPortraitOffsets;             // To be entered in inspector. Individual offsets for each character's portrait sprite.
        [SerializeField] private GameObject modeSelectButtonPrefab;                       // Asset reference. To be instantiated in runtime, set up, and assigned to menu controller.
        [SerializeField] private Transform buttonsParent;                 // Scene reference. To be positioned, and have buttons parented to.
        [SerializeField] private TextMeshProUGUI gameModeNameTMP;
        [SerializeField] private TextMeshProUGUI gameModeDescriptionTMP;
        [SerializeField] private PlayerConfigManager playerConfigManager;
        
        [SerializeField] private GameObject modeSelectCanvas;
        
        public GameModeSO CurrentGameMode;
        
        public List<LobbyPlayerController> LobbyPlayerControllers { get; set; }
        
        public UnityEvent mainMenuRequested = new();
        public UnityEvent startGameRequested = new();
        
        private void OnEnable()
        {
            modeSelectCanvas.gameObject.SetActive(true);
            
            SetUpSelectedCharacterUI();
        }

        private void OnDisable()
        {
            modeSelectCanvas.gameObject.SetActive(false);
        }

        private void Start()
        {
            CreateGameModeButtons();
        }

        private void CreateGameModeButtons()
        {
            var modeSelectButtons = new List<ModeSelectButton>();
            foreach (GameModeSO gameModeSO in gameModeSOs)
            {
                GameObject buttonObject = Instantiate(modeSelectButtonPrefab, buttonsParent.transform);

                buttonObject.name = $"{gameModeSO.displayName} Mode Select Button";

                var modeSelectButton = buttonObject.GetComponent<ModeSelectButton>();

                // TODO: We should make this logic into a utility
                if (modeSelectButton == null)
                {
                    throw new Exception(
                        $"{nameof(modeSelectButtonPrefab)} does not contain {nameof(ModeSelectButton)} component;");
                }
                
                modeSelectButtons.Add(modeSelectButton);

                modeSelectButton.Initialise(gameModeSO);
                modeSelectButton.clicked.AddListener(OnModeButtonClicked);
                modeSelectButton.selected.AddListener(OnModeButtonSelected);
            }

            modeSelectButtons[0].GetComponent<Button>().Select();
            
            // Update textmesh elements
            UpdateGameModeDescription(gameModeSOs[0]);
        }

        private void OnModeButtonClicked(Button<CharacterSelectDialogue> button)
        {
            var gameModeSO = ((ModeSelectButton) button).GameModeSO;
            
            CurrentGameMode = gameModeSO;

            startGameRequested.Invoke();
        }

        private void OnModeButtonSelected(MenuButton<CharacterSelectDialogue> menuButton)
        {
            var gameModeSO = ((ModeSelectButton) menuButton).GameModeSO;
            
            UpdateGameModeDescription(gameModeSO);
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

        public void TryExitModeSelect()
        {
            mainMenuRequested.Invoke();
        }

        public void UpdateGameModeDescription(GameModeSO gameModeSO)
        {
            gameModeNameTMP.text = gameModeSO.displayName;
            gameModeDescriptionTMP.text = gameModeSO.description;
        }
    }
}