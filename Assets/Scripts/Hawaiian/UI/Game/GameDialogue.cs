using System.Collections.Generic;
using Hawaiian.Game;
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

        protected override void Awake()
        {
            base.Awake();
            
            DisplayControls();
        }

        private void DisplayControls()
        {
            tutorialBackground.SetActive(true);

            var gameMode = gameManager.CurrentGameMode;
            
            Instantiate(gameMode.ControlsInstructionsDialoguePrefab, transform.parent);
            Instantiate(gameMode.TutorialDialoguePrefab, transform.parent);
        }

        public void Pause()
        {
            // TODO: Just create an isPromoted variable
            if (!canvasGroup.interactable) return;
            
            Instantiate(pauseMenuDialoguePrefab, transform.parent);
        }
    }
}