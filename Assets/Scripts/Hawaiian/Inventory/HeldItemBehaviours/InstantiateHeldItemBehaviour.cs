using System.Collections.Generic;
using Hawaiian.Inventory.ItemBehaviours;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public abstract class InstantiateHeldItemBehaviour<T> : HeldItemBehaviour where T : ItemBehaviour
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
        
        // TODO: Should not depend on Projectile
        protected virtual bool CanUseProjectile()
        {
            if (_projectileInstance == null) return true;
            if (!_projectileInstance.GetComponent<Projectile>()) return true;
            if (_projectileInstance.GetComponent<Projectile>().IsOnWall()) return true;

            return false;
        }
        
        protected virtual void UseItem(List<GameObject> projectiles = null)
        {
            if (projectiles == null) return;

            for (var i = 0; i < projectiles.Count; i++)
            {
                var p = projectiles[i];

                var itemBehaviour = p.GetComponent<T>();
                
                itemBehaviour.BaseInitialise(UnitPlayer, Item.DrawSpeed, Item.KnockbackDistance);
                
                InitialiseInstantiatedItemBehaviour(itemBehaviour, i);
                
                if (p.GetComponent<HitUnit>())
                    p.GetComponent<HitUnit>()
                        .Initialise(UnitPlayer, Cursor.transform.position - transform.position);

                UnitPlayer.transform.GetComponent<UnitAnimator>()
                    .UseItem(UnitAnimationState.Throw, Cursor.transform.localPosition, false);
            }
        }
        
        protected virtual void InitialiseInstantiatedItemBehaviour(T itemBehaviour, int i) { }
    }
}