using System;
using System.Collections.Generic;
using Hawaiian.UI.MainMenu;
using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.Game
{
    public class GameDialogue : Dialogue
    {
        [SerializeField] private GameObject inventoryUIPrefab;
        [SerializeField] private Transform uiParent;
        [SerializeField] private Transform uiParentForFourPlayers;
        [SerializeField] private GameObject tutorialDialoguePrefab;
        [SerializeField] private GameObject controlsInstructionsDialoguePrefab;
        [SerializeField] private MainMenuController mainMenuController;
        [SerializeField] private MainMenuButtonFunctions pauseMenuController;
        [SerializeField] private GameObject tutorialBackground;

        private int _inventoryCount = 0;
        private List<GameObject> inventoryGameObjects = new();
        
        protected override void OnClose() { }

        protected override void OnPromote()
        {
            tutorialBackground.SetActive(false);
            
            if (mainMenuController.enabled)
                mainMenuController.CursorToStartingState();
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

        protected override void Awake()
        {
            base.Awake();
            
            DisplayControls();
        }

        private void OnEnable()
        {
            pauseMenuController.displayControlsSelected.AddListener(DisplayControls);
        }

        private void OnDisable()
        {
            pauseMenuController.displayControlsSelected.RemoveListener(DisplayControls);
        }

        private void DisplayControls()
        {
            tutorialBackground.SetActive(true);
            
            Instantiate(controlsInstructionsDialoguePrefab, transform.parent);

            Instantiate(tutorialDialoguePrefab, transform.parent);
        }
    }
}