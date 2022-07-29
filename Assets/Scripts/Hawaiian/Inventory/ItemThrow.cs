using System.Collections.Generic;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory
{
    public class ItemThrow : ItemInstantiate
    {
        [SerializeField] private IUnitGameEvent _removeItem;
        
        public UnityEvent throwableArcPositionsUpdated = new();
        
        public List<Vector2> throwableArcPositions = new List<Vector2>();
        
        private void FixedUpdate()
        {
            if (_isHoldingAttack)
            {
                UpdateThrowableArcPositions();
            }
        }

        private void UpdateThrowableArcPositions()
        {
            throwableArcPositions = BezierCurve.QuadraticBezierCurvePoints(
                transform.position,
                transform.position + new Vector3(0.5f, 2, 0),
                cursor.transform.position,
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
                        .BaseInitialise(_playerReference, item.DrawSpeed, item.KnockbackDistance);

                    switch (p.GetComponent<T>())
                    {
                        case Throwable:
                            p.GetComponent<T>().Initialise(throwableArcPositions.ToArray(), item.ItemSprite,
                                item.SticksOnWall);
                            AudioManager.audioManager.PlayWeapon(9);
                            _removeItem.Raise(_playerReference);
                            break;
                    }

                    if (p.GetComponent<HitUnit>())
                        p.GetComponent<HitUnit>()
                            .Initialise(_playerReference, cursor.transform.position - transform.position);


                    _playerReference.transform.GetComponent<UnitAnimator>()
                        .UseItem(UnitAnimationState.Throw, cursor.transform.localPosition, false);

                    index++;
                });
            }
        }
    }
}