﻿using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Hawaiian.Unit
{
    public class DealKnockback : MonoBehaviour
    {
        [SerializeField] private new Rigidbody2D rigidbody2D;
        
        private int knockbackDistance;
        private IUnit user;
        private Vector2 defaultDirection;
        private bool useDefaultDirection;

        public void Initialise(int knockbackDistance, IUnit user) 
        {
            this.knockbackDistance = knockbackDistance;
            this.user = user;
        }

        public void Initialise(int knockbackDistance, IUnit user, Vector2 direction)
        {
            Initialise(knockbackDistance, user);

            defaultDirection = direction;
            useDefaultDirection = true;
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            // TODO: Duplicate code. See DamageIndicator.OnTriggerEnter2D
            if (col.gameObject.GetComponent<Unit>() is IUnit)
            {
                //Yucky 
                IUnit target = (IUnit) col.gameObject.GetComponent<Unit>();

                if (target == user)
                    return;

                Vector2 direction;

                // TODO: This is really jank, but I don't want to rework Projectile and cause merge conflicts
                if (useDefaultDirection)
                    direction = defaultDirection;
                else if (rigidbody2D == null)
                {
                    if (GetComponent<Projectile>())
                        direction = GetComponent<Projectile>().Direction;
                    else
                        direction = defaultDirection;

                }
                else
                {
                    direction = rigidbody2D.velocity.normalized;
                }

                // TODO: Move knockback to IUnit
                col.gameObject.GetComponent<UnitPlayer>().KnockBack(direction, knockbackDistance);
            }
        }
    }
}