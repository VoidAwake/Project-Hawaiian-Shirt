using System.Collections.Generic;
using Hawaiian.Inventory.ItemBehaviours;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public abstract class InstantiateHeldItemBehaviour<T> : HeldItemBehaviour where T : ItemBehaviour
    {
        [SerializeField] private UnityEvent itemUsed;

        protected readonly List<T> InstantiatedItemBehaviours = new();
        
        protected override void UseItemActionPerformed(InputAction.CallbackContext value)
        {
            base.UseItemActionPerformed(value);
            UnitPlayer.IsSlowed = true;
        }

        protected override void UseItemActionCancelled(InputAction.CallbackContext value)
        {
            base.UseItemActionCancelled(value);
            
            UnitPlayer.IsSlowed = false;
            
            if (!CanUseItem()) return;

            InstantiatedItemBehaviours.Clear();

            for (int i = 0; i < NumberOfObjectsToInstantiate(); i++)
            {
                // TODO: what
                var instantiationPosition = transform.position + 0.1f * (Cursor.transform.position - transform.position);
                
                var itemBehaviourObject = Instantiate(Item.ProjectileInstance, instantiationPosition, Quaternion.identity);

                var itemBehaviour = itemBehaviourObject.GetComponent<T>();
                
                InstantiatedItemBehaviours.Add(itemBehaviour);
            }

            UseItem(InstantiatedItemBehaviours);

            Cursor.LerpToReset();
        }
        
        protected virtual bool CanUseItem()
        {
            return true;
        }
        
        protected virtual void UseItem(List<T> itemBehaviours)
        {
            for (var i = 0; i < itemBehaviours.Count; i++)
            {
                var itemBehaviour = itemBehaviours[i];
                
                itemBehaviour.BaseInitialise(UnitPlayer, Item.DrawSpeed, Item.KnockbackDistance);
                
                InitialiseInstantiatedItemBehaviour(itemBehaviour, i);
                
                if (itemBehaviour.GetComponent<HitUnit>())
                    itemBehaviour.GetComponent<HitUnit>()
                        .Initialise(UnitPlayer, Cursor.transform.position - transform.position);

                UnitPlayer.transform.GetComponent<UnitAnimator>()
                    .UseItem(UnitAnimationState.Throw, Cursor.transform.localPosition, false);
            }
            
            itemUsed.Invoke();
        }
        
        protected virtual void InitialiseInstantiatedItemBehaviour(T itemBehaviour, int i) { }

        protected virtual int NumberOfObjectsToInstantiate()
        {
            return 1;
        }
    }
}