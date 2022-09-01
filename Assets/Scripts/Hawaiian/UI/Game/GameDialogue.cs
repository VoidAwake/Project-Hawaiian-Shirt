using System.Collections.Generic;
using Hawaiian.Game;
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

        private int _inventoryCount = 0;
        private List<GameObject> inventoryGameObjects = new();
        
        protected override void OnClose() { }

        protected override void OnPromote()
        {
            tutorialBackground.SetActive(false);
        }

        protected override void OnDemote() { }

        public void OnInventoryAdded(Inventory.Inventory inventory)
        {
            var inventoryGameObject = Instantiate(inventoryUIPrefab, _inventoryCount >= 2 ? uiParentForFourPlayers : uiParent);
            inventoryGameObjects.Add(inventoryGameObject);
            
            inventoryGameObject.GetComponent<InventoryUI>().inv = inventory;
            inventoryGameObject.GetComponentInChildren<ScoreUI>().inventory = inventory;
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
            var gameMode = gameManager.CurrentGameMode;

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