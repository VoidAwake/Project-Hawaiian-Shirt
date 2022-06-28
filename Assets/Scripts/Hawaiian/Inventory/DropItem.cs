using UnityEngine;

namespace Hawaiian.Inventory
{
    public class DropItem : HitEffect
    {
        public override void OnHit(Unit.Unit unit, Vector2 direction)
        {
            // TODO: Should find a better way to access inventory
            var inventoryController = unit.GetComponentInChildren<InventoryController>();

            if (inventoryController != null)
            {
                if (inventoryController.CurrentItem.IsDepositor )
                    return;
                
                inventoryController.DropRandom(direction);
            }
        }
    }

}