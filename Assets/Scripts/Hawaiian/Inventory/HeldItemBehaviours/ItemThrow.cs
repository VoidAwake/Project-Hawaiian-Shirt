using System.Collections.Generic;
using Hawaiian.Inventory.ItemBehaviours;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class ItemThrow : ItemInstantiate
    {
        public UnityEvent throwableArcPositionsUpdated = new();
        
        public List<Vector2> throwableArcPositions = new List<Vector2>();

        public UnityEvent threw = new();
        
        private void FixedUpdate()
        {
            if (UseItemActionHeld)
            {
                UpdateThrowableArcPositions();
            }
        }

        private void UpdateThrowableArcPositions()
        {
            throwableArcPositions = BezierCurve.QuadraticBezierCurvePoints(
                transform.position,
                transform.position + new Vector3(0.5f, 2, 0),
                Cursor.transform.position,
                200
            );
            
            throwableArcPositionsUpdated.Invoke();
        }

        protected override void UseItem(List<GameObject> projectiles = null)
        {
            base.UseItem(projectiles);
            
            UseItem<Throwable>(projectiles);
        }

        private void UseItem<T>(List<GameObject> projectiles = null) where T : ItemBehaviour
        {
            int index = 0;

            if (projectiles != null)
            {
                projectiles.ForEach(p =>
                {
                    p.GetComponent<T>()
                        .BaseInitialise(UnitPlayer, Item.DrawSpeed, Item.KnockbackDistance);

                    switch (p.GetComponent<T>())
                    {
                        case Throwable:
                            p.GetComponent<T>().Initialise(throwableArcPositions.ToArray(), Item.ItemSprite,
                                Item.SticksOnWall);
                            DestroyHeldItem();
                            
                            threw.Invoke();
                            break;
                    }

                    if (p.GetComponent<HitUnit>())
                        p.GetComponent<HitUnit>()
                            .Initialise(UnitPlayer, Cursor.transform.position - transform.position);


                    UnitPlayer.transform.GetComponent<UnitAnimator>()
                        .UseItem(UnitAnimationState.Throw, Cursor.transform.localPosition, false);

                    index++;
                });
            }
        }
    }
}