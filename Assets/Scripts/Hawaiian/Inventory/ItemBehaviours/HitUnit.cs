﻿using System.Collections.Generic;
using Hawaiian.Unit;
using UnityEngine;

namespace Hawaiian.Inventory.ItemBehaviours
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
            
            if (unit is not IUnit target) return;

            if (_overrideDirectionForBombs)
            {
                direction = target.GetPosition() - transform.position;
            }
            
            // TODO: Copied from a duplicate DealKnockback script. Needs sorting out.
            // TODO: defaultDirection was passed in to Initialise
            // TODO: This is really jank, but I don't want to rework Projectile and cause merge conflicts
            // if (useDefaultDirection)
            //     direction = defaultDirection;
            // else if (rigidbody2D == null)
            // {
            //     if (GetComponent<Projectile>())
            //         direction = GetComponent<Projectile>().Direction;
            //     else if (GetComponent<Bomb>())
            //     {
            //         direction = col.gameObject.transform.position - GetComponent<Bomb>().transform.position;
            //     } else
            //         direction = defaultDirection;
            //
            // }
            // else
            // {
            //     direction = rigidbody2D.velocity.normalized;
            // }

            // if (gameObject.GetComponent<Projectile>() != null)
            // {
                // TODO: Reconsider the logic of this
                // if (!gameObject.GetComponent<Projectile>().WasParried)
                // {
                    if (target == user || oldTargets.Contains(target))
                        return;
                // }
                // else
                // {
                    // direction = -direction;
                // }
            // }

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