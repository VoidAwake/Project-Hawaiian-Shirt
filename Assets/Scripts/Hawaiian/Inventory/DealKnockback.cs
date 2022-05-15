using Hawaiian.Unit;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class DealKnockback : HitEffect
    {
        [SerializeField] private int knockbackDistance;
        
        private IUnit user;

        public override void OnHit(Unit.Unit unit, Vector2 direction)
        {
            // TODO: Move knockback to IUnit

            if (GetComponent<DamageIndicator>())
                knockbackDistance = GetComponent<DamageIndicator>()._knockbackDistance;
            
            unit.GetComponent<UnitPlayer>().KnockBack(direction, knockbackDistance);
        }
    }
}