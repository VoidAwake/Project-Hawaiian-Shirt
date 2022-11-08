using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Hawaiian.Inventory;
using Hawaiian.Level;
using MoreLinq;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Game.GameModes.Tutorial
{
    [CreateAssetMenu(order = 0, menuName = "Hawaiian/Managers/GameModeManager/TutorialModeManager")]
    public class TutorialModeManager : ModeManager<TutorialSceneReference>
    {
        [SerializeField] private List<GameObject> treasureSpawners;
        [SerializeField] private Item treasureItem;
        [SerializeField] private Item weaponItem;
        [SerializeField] private GameObject droppedItemPrefab;
        [SerializeField] private GameObject targetDummy;

        private readonly Dictionary<PlayerConfig, InventoryController> inventoryControllers = new();

        [NonSerialized] public UnityEvent textChanged = new();
        private string text;

        public string Text
        {
            get => text;
            private set
            {
                text = value;
                textChanged.Invoke();
            }
        }

        protected override void OnPlayerJoined(PlayerConfig playerConfig)
        {
            // TODO: Duplicate code. See GameDialogue.OnPlayerJoined.
            var inventoryController = playerConfig.playerInput.GetComponentInChildren<InventoryController>();

            inventoryControllers.Add(playerConfig, inventoryController);

            // TODO: Remove listener
            inventoryController.inv.inventoryChanged.AddListener(UpdateWinningPlayers);
            
            base.OnPlayerJoined(playerConfig);
        }
        
        protected override float PlayerConfigScore(PlayerConfig playerConfig)
        {
            return inventoryControllers[playerConfig].Score;
        }


        public override void ListenToLevel()
        {
            base.ListenToLevel();
            
            Tutorial();
        }
        
        private async void Tutorial()
        {
            // TODO: First instruction should be how to move.

            Text = "Pick up treasures to increase your score";
            
            // TODO: Say "pick up treasures to increase score"
            // TODO: Show the pickup control

            // await UniTask.Delay(3000);
            
            foreach (var spawnPoint in playerManager.spawnPoints)
            {
                var droppedItemObject = Instantiate(droppedItemPrefab, spawnPoint.transform);
                DroppedItem droppedItem = droppedItemObject.GetComponent<DroppedItem>();
                // droppedItem.Item = treasureItem;
            }

            await UniTask.WaitUntil(() => inventoryControllers.All(i => i.Value.Score == 10));
            
            FindObjectsOfType<Door>().ForEach(door => door.Unlock());

            Text = "Use weapons to make players drop their treasure";
            
            foreach (var spawnPoint in playerManager.spawnPoints)
            {
                var droppedItemObject = Instantiate(droppedItemPrefab, spawnPoint.transform);
                DroppedItem droppedItem = droppedItemObject.GetComponent<DroppedItem>();
                droppedItem.Item = weaponItem;
            }
            
            foreach (var spawnPoint in playerManager.spawnPoints)
            {
                var droppedItemObject = Instantiate(targetDummy, spawnPoint.transform);
            }

            // TODO: Say "use weapons to make things drop treasure"
            // TODO: Show the attack control
            
            // TODO: Need the practice dummies to drop treasure
            await UniTask.WaitUntil(() => inventoryControllers.All(i => i.Value.Score == 20));
        }
    }
}