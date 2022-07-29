using Hawaiian.Inventory.ItemBehaviours;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    // TODO: Merge with Shield?
    public class ItemShield : HeldItemBehaviour
    {
        [SerializeField] private IUnitGameEvent _parryOccured;
        [SerializeField] private GameObject shieldColliderPrefab;
        [SerializeField] private Shield _shieldReference;
        private SpriteRenderer heldItemSpriteRenderer;

        protected override void Initialise(ItemHolder itemHolder)
        {
            base.Initialise(itemHolder);
            
            _shieldReference.Initialise(Item.ParryWindow,
                new[] { Item.ParryPercentageUpperLimit, Item.ParryPercentageLowerLimit }, heldItemSpriteRenderer,
                new[] { Item.ShieldDown, Item.ShieldUp }, _parryOccured, shieldColliderPrefab,
                Item.TimeTillParry, UnitPlayer, Cursor.transform);
        }

        protected override void UseItemActionPerformed(InputAction.CallbackContext value)
        {
            if (_shieldReference.CanParry())
                _shieldReference.LiftShield();
        }
    }
}