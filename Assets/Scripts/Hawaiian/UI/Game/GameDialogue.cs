using System.Collections.Generic;
using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.Game
{
    public class GameDialogue : Dialogue
    {
        [SerializeField] private GameObject inventoryUIPrefab;
        [SerializeField] private Transform uiParent;
        [SerializeField] private Transform uiParentForFourPlayers;

        private int _inventoryCount = 0;
        private List<GameObject> inventoryGameObjects = new();
        
        protected override void OnClose() { }

        protected override void OnPromote() { }

        protected override void OnDemote() { }

        public void OnInventoryAdded(Inventory.Inventory inventory)
        {
            var inventoryGameObject = Instantiate(inventoryUIPrefab, _inventoryCount >= 2 ? uiParentForFourPlayers : uiParent);
            inventoryGameObjects.Add(inventoryGameObject);
            
            inventoryGameObject.GetComponent<InventoryUI>().inv = inventory;
            inventoryGameObject.GetComponentInChildren<ScoreUI>().inventory = inventory;
            _inventoryCount++;
        }
        
        
    }
}