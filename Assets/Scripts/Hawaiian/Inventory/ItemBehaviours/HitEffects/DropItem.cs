using UnityEngine;

namespace Hawaiian.Inventory.ItemBehaviours.HitEffects
{
    // TODO: Separate into two classes, DropItem and DropCash
    public class DropItem : HitEffect
    {
        public override void OnHit(Unit.Unit unit, Vector2 direction)
        {
            // TODO: Should find a better way to access inventory
            var inventoryController = unit.GetComponentInChildren<InventoryController>();
            var score = unit.GetComponentInChildren<Score>();

            if (inventoryController != null)
            {
                //inventoryController.DropRandom(direction);
            }

            if (score != null)
            {
                score.DropCash();
            }
        }
    }
}