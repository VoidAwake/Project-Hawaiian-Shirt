using System;
using System.Collections.Generic;
using Hawaiian.Game;
using Hawaiian.Game.GameModes;
using Hawaiian.Inventory;
using Hawaiian.Utilities;
using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.Game
{
    public class GameDialogue : Dialogue
    {
        [SerializeField] private GameObject inventoryUIPrefab;
        [SerializeField] private Transform uiParent;
        [SerializeField] private Transform uiParentForFourPlayers;
        [SerializeField] private GameObject tutorialBackground;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject pauseMenuDialoguePrefab;
        [SerializeField] private GameEvent startGameEvent;
        [SerializeField] private GameEvent startPlayerSpawnsEvent;
        [SerializeField] private PlayerManager playerManager;

        private int _inventoryCount = 0;
        private List<GameObject> inventoryGameObjects = new();
        
        protected override void OnClose() { }

        protected override void OnPromote()
        {
            tutorialBackground.SetActive(false);
        }

        protected override void OnDemote() { }

        public void OnEnable()
        {
            // TODO: Duplicate code. See ModeManager<T>.LoadRandomLevel.
            // TODO: Come back to this
            playerManager = FindObjectOfType<PlayerManager>();
            
            playerManager.playerJoined.AddListener(OnPlayerJoined);
        }

        public void OnDisable()
        {
            playerManager.playerJoined.RemoveListener(OnPlayerJoined);
        }

        private void OnPlayerJoined(PlayerConfig playerConfig)
        {
            var inventoryGameObject = Instantiate(inventoryUIPrefab, _inventoryCount >= 2 ? uiParentForFourPlayers : uiParent);
            
            inventoryGameObjects.Add(inventoryGameObject);
            
            // TODO: Duplicate code. See ModeManager.OnPlayerJoined.
            var inventory = playerConfig.playerInput.GetComponentInChildren<InventoryController>().inv;
            var score = playerConfig.playerInput.GetComponentInChildren<Score>();
            
            inventoryGameObject.GetComponent<InventoryUI>().Initialise(playerConfig, inventory);
            
            // TODO: Set this using an Initialise function, or from InventoryUI
            inventoryGameObject.GetComponentInChildren<ScoreUI>().score = score;
            
            _inventoryCount++;
        }

        protected override void Start()
        {
            base.Start();
            
            // Must be called in Start to give PlayerManager time to find the PlayerInputManager
            DisplayControls();
        }

        private void DisplayControls()
        {
            var gameMode = ModeManager.CurrentModeManager;

            if (gameMode.ControlsInstructionsDialoguePrefab != null || gameMode.TutorialDialoguePrefab != null)
            {
                tutorialBackground.SetActive(true);
                
                if (gameMode.ControlsInstructionsDialoguePrefab != null)
                    Instantiate(gameMode.ControlsInstructionsDialoguePrefab, transform.parent);

                if (gameMode.TutorialDialoguePrefab != null)
                    Instantiate(gameMode.TutorialDialoguePrefab, transform.parent);
            }
            else
            {
                startGameEvent.Raise();
                startPlayerSpawnsEvent.Raise();
            }
        }

        public void Pause()
        {
            // TODO: Just create an isPromoted variable
            if (!canvasGroup.interactable) return;
            
            Instantiate(pauseMenuDialoguePrefab, transform.parent);
        }
    }
}