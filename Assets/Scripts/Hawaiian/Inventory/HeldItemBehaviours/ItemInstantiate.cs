using System.Collections.Generic;
using Hawaiian.Inventory.ItemBehaviours;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class ItemInstantiate : HeldItemBehaviour
    {
        private GameObject _projectileInstance;

        protected override void UseItemActionCancelled(InputAction.CallbackContext value)
        {
            base.UseItemActionCancelled(value);
            
            if (!CanUseProjectile()) return;

            var projectiles = new List<GameObject>();

            for (int i = Item.ProjectileAmount == 0 ? -1 : 0;
                 i < Item.ProjectileAmount;
                 i++)
            {
                _projectileInstance = Instantiate(Item.ProjectileInstance,
                    transform.position + 0.1f * (Cursor.transform.position - transform.position), Quaternion.identity);
                projectiles.Add(_projectileInstance);

                if (i == -1)
                    break;
            }

            UseItem(projectiles);

            Cursor.LerpToReset();
        }
        
        protected virtual bool CanUseProjectile()
        {
            if (_projectileInstance == null) return true;
            if (!_projectileInstance.GetComponent<Projectile>()) return true;
            if (_projectileInstance.GetComponent<Projectile>().IsOnWall()) return true;

            return false;
        }
        
        // TODO: Still need to have another look at these generics
        // private void UseItem<T>(List<GameObject> projectiles = null) where T : ItemBehaviour
        protected virtual void UseItem(List<GameObject> projectiles = null)
        {
        }
    }
}