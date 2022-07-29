using Hawaiian.Unit;
using UnityEngine;

namespace Hawaiian.Inventory.ItemBehaviours.HitEffects
{
    public class DealKnockback : HitEffect
    {
        [SerializeField] private int knockbackDistance;
        
        private IUnit user;

        public override void OnHit(Unit.Unit unit, Vector2 direction)
        {
            // TODO: Move knockback to IUnit

            if (GetComponent<DamageIndicator>())
                knockbackDistance = GetComponent<DamageIndicator>().KnockbackDistance;

            if (unit.GetComponentInChildren<Shield>())
                unit.GetComponentInChildren<Shield>().IsHit = true;
            
            
            unit.GetComponent<UnitPlayer>().KnockBack(direction, knockbackDistance);
        }
    }
}