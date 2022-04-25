using System.Collections.Generic;
using UI.Core;
using UnityEngine;

namespace Hawaiian.UI.Game
{
    public class GameDialogue : Dialogue
    {
        [SerializeField] private GameObject inventoryUIPrefab;
        [SerializeField] private Transform uiParent;

        private List<GameObject> inventoryGameObjects = new();
        
        protected override void OnClose() { }

        protected override void OnPromote() { }

        protected override void OnDemote() { }

        public void OnInventoryAdded(Inventory.Inventory inventory)
        {
            var inventoryGameObject = Instantiate(inventoryUIPrefab, uiParent);
            
            inventoryGameObjects.Add(inventoryGameObject);
            
            inventoryGameObject.GetComponent<InventoryUI>().inv = inventory;
        }
    }
}