using System;
using System.Collections.Generic;
using Hawaiian.Unit;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class HitUnit : MonoBehaviour
    {
        [SerializeField] private List<HitEffect> hitEffects;
        [SerializeField] private bool _overrideDirectionForBombs;

        private readonly List<IUnit> oldTargets = new List<IUnit>();
        private IUnit user;
        private Vector2 direction;

        //Just alright it happens
        public bool isActive { get; set; }

        public void Initialise(IUnit user, Vector2 direction)
        {
            this.user = user;
            this.direction = direction;
            isActive = true;
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            if (GetComponent<Projectile>() != null)
            {
                if (GetComponent<Projectile>().IsOnWall())
                    return;
            }

            // TODO: Duplicate code. See DamageIndicator.OnTriggerEnter2D
            var unit = col.gameObject.GetComponent<Unit.Unit>();
            if (unit is IUnit)
            {
                //Yucky 
                IUnit target = (IUnit) unit;

                if (_overrideDirectionForBombs)
                {
                    direction = target.GetPosition() - transform.position;
                }

                if (target == user || oldTargets.Contains(target))
                    return;

                if (unit.isInvincible) return;

                oldTargets.Add(target);

                foreach (var hitEffect in hitEffects)
                {
                    hitEffect.OnHit(unit, direction);
                }
            }
        }
    }
}