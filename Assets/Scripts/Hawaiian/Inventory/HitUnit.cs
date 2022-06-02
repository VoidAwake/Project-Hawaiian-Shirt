using System;
using System.Collections.Generic;
using Hawaiian.Unit;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class HitUnit : MonoBehaviour
    {
        [SerializeField] private List<HitEffect> hitEffects;

        private readonly List<IUnit> oldTargets = new List<IUnit>();
        private IUnit user;
        private Vector2 direction;

        //Just alright it happens
        [SerializeField] private bool _overrideDirectionForBombs;

        public void Initialise(IUnit user, Vector2 direction)
        {
            this.user = user;
            this.direction = direction;
        }


        public void OnTriggerEnter2D(Collider2D col)
        {
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

                if (gameObject.GetComponent<Projectile>() != null)
                {
                    if (!gameObject.GetComponent<Projectile>().WasParried)
                    {
                        if (target == user || oldTargets.Contains(target))
                            return;
                    }
                    else
                    {
                        direction = -direction;
                    }
                }

                if (unit.isInvincible) return;

                //Used if the user has parried sucessfully
                if (unit.OverrideDamage)
                {
                    unit.OverrideDamage = false;
                    return;
                }

                oldTargets.Add(target);

                foreach (var hitEffect in hitEffects)
                {
                    hitEffect.OnHit(unit, direction);
                }
            }
        }
    }
}