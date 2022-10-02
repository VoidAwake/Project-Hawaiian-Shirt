using Hawaiian.Inventory.HeldItemBehaviours;
using Hawaiian.Unit;
using UnityEngine;

namespace Hawaiian.Inventory.ItemBehaviours.HitEffects
{
    public class DealKnockback : HitEffect
    {
        [SerializeField] private int knockbackDistance;
        
        private IUnit user;
        public bool induceInvincibility = true;

        public override void OnHit(Unit.Unit unit, Vector2 direction)
        {
            if (GetComponent<DamageIndicator>())
                knockbackDistance = GetComponent<DamageIndicator>().KnockbackDistance;

            if (unit.GetComponentInChildren<ItemShield>())
                unit.GetComponentInChildren<ItemShield>().IsHit = true;
            
            // TODO: Move knockback to IUnit
            unit.GetComponent<UnitPlayer>().KnockBack(direction, knockbackDistance, induceInvincibility);
            //print("On hit! Induce invincibility? " + induceInvincibility);
            // :-(
        }
    }
}