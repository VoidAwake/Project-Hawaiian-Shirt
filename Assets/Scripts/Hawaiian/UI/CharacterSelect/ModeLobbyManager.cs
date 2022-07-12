using System;
using System.Collections.Generic;
using Hawaiian.Game;
using Hawaiian.UI.Game;
using Hawaiian.UI.MainMenu;
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
        [SerializeField] private GameObject buttonPrefab;                       // Asset reference. To be instantiated in runtime, set up, and assigned to menu controller.
        [SerializeField] private GridLayoutGroup buttonsParent;                 // Scene reference. To be positioned, and have buttons parented to.
        [SerializeField] private TextMeshProUGUI gameModeNameTMP;
        [SerializeField] private TextMeshProUGUI gameModeDescriptionTMP;
        [SerializeField] private PlayerConfigManager playerConfigManager;
        
        [SerializeField] private GameObject modeSelectCanvas;
        public MainMenuController menuController;                               // Scene reference. To assign button refernce to, and redirect player inputs.
        
        public GameModeSO CurrentGameMode;
        
        public List<LobbyPlayerController> LobbyPlayerControllers { get; set; }
        
        public UnityEvent mainMenuRequested = new();
        
        private void OnEnable()
        {
            modeSelectCanvas.gameObject.SetActive(true);
            menuController.disabled = false;
            
            SetUpSelectedCharacterUI();
            
            // Format buttons
            //buttonsParent.enabled = !enable;
            //menuController.cursor.gameObject.SetActive(enable);
        }

        private void OnDisable()
        {
            modeSelectCanvas.gameObject.SetActive(false);
            menuController.disabled = true;
        }

        private void Start()
        {
            CreateGameModeButtons();
        }

        private void CreateGameModeButtons()
        {
            // Create buttons for mode select screen
            Button[] buttons = new Button[gameModeSOs.Length];
            int counter = 0;
            foreach (GameModeSO gameModeSO in gameModeSOs)
            {
                GameObject button = Instantiate(buttonPrefab, buttonsParent.transform);                                             // Create and parent button
                float yPos = counter * -130.0f;
                button.transform.localPosition = new Vector2(0.0f, yPos);
                if (counter == 0) menuController.cursor.transform.localPosition = button.transform.localPosition;                   // Set cursor to initial position, for first element
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gameModeSO.displayName;
                MainMenuButtonFunctions buttonFunction = button.gameObject.AddComponent<MainMenuButtonFunctions>();                 // Add button function component
                buttonFunction.SetButtonFunction(0, gameModeSO);                            // Set button's function to load build index associated with game mode
                buttonFunction.isModeSelectButton = true;                                                                           // Set button to tell this script to update selection's decription
                buttons[counter] = button.GetComponent<Button>();
                counter++;
            }
            menuController.buttons = buttons;                                                                                       // Assign reference to new buttons in menu controller
            buttonsParent.transform.localPosition = new Vector2(-200.0f,
                ((gameModeSOs.Length * 100.0f) + ((gameModeSOs.Length - 1) * 30.0f)) / 2.0f - 50.0f);                                   // Position buttons' parent so that they're centred on screen
            menuController.cursor.transform.parent.localPosition = new Vector2(-200.0f,
                ((gameModeSOs.Length * 100.0f) + ((gameModeSOs.Length - 1) * 30.0f)) / 2.0f - 50.0f);                                   // Gotta move parent of cursor too, because awesome code

            // Update textmesh elements
            UpdateGameModeDescription(0);
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

        public void UpdateGameModeDescription(int index)
        {
            gameModeNameTMP.text = gameModeSOs[index].displayName;
            gameModeDescriptionTMP.text = gameModeSOs[index].description;
        }
    }
}