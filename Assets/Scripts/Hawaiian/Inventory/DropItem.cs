using System.Collections.Generic;
using Hawaiian.Unit;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class DropItem : MonoBehaviour
    {
        private IUnit user;
        private readonly List<IUnit> oldTargets = new List<IUnit>();

        public void Initialise(IUnit user) 
        {
            this.user = user;
        }
        
        public void OnTriggerEnter2D(Collider2D col)
        {
            // TODO: Duplicate code. See DamageIndicator.OnTriggerEnter2D
            if (col.gameObject.GetComponent<Unit.Unit>() is IUnit)
            {
                //Yucky 
                IUnit target = (IUnit) col.gameObject.GetComponent<Unit.Unit>();

                if (target == user || oldTargets.Contains(target))
                    return;
                
                oldTargets.Add(target);
                
                // TODO: Should find a better way to access inventory
                var inventoryController = col.GetComponentInChildren<InventoryController>();

                if (inventoryController != null)
                {
                    inventoryController.DropRandom();
                }
            }
        }
    }
}