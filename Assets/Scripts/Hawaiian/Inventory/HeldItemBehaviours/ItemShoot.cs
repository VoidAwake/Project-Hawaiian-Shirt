using System.Collections.Generic;
using Hawaiian.Inventory.ItemBehaviours;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class ItemShoot : ItemInstantiate
    {
        public UnityEvent targetCountChanged = new();
        public UnityEvent multiShotTargetsUpdated = new();
        
        private bool collisionFlag = false;
        
        public Vector3[] _multiShotTargets;
        private int targetCount;

        protected override void Initialise(ItemHolder itemHolder)
        {
            base.Initialise(itemHolder);
            
            TargetCount = Item.ProjectileAmount == 0 ? 1 : Item.ProjectileAmount;
        }

        public int TargetCount
        {
            get => targetCount;
            private set
            {
                targetCount = value;
                targetCountChanged.Invoke();
                UpdateMultiShotTargets();
            }
        } 
        
        private void FixedUpdate()
        {
            if (UseItemActionHeld)
            {
                UpdateMultiShotTargets();
            }
        }

        private void UpdateMultiShotTargets()
        {
            if (_multiShotTargets.Length != targetCount)
            {
                _multiShotTargets = new Vector3[targetCount];
            }
            
            if (collisionFlag)
                return;

            var direction = (Cursor.transform.position - transform.position).normalized;
         
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle < 0) angle = 360 - angle * -1;

            for (var i = 0; i < TargetCount; i++)
            {
                // Increment the angle for each target
                var currentAngle = angle + 20f * i - (TargetCount - 1) / (float) 2 * 20f;

                var radians = currentAngle * Mathf.Deg2Rad;

                var x = Mathf.Cos(radians);
                var y = Mathf.Sin(radians);
                var targetPos = transform.position + new Vector3(x, y, 0f) * Cursor.CurrentRad;

                _multiShotTargets[i] = targetPos;
            }
            
            multiShotTargetsUpdated.Invoke();
        }

        protected override void UseItem(List<GameObject> projectiles = null)
        {
            base.UseItem(projectiles);
            
            UseItem<Projectile>(projectiles);
        }
        
        private void UseItem<T>(List<GameObject> projectiles = null) where T : ItemBehaviour
        {
            collisionFlag = false;

            int index = 0;

            if (projectiles != null)
            {
                projectiles.ForEach(p =>
                {
                    p.GetComponent<T>()
                        .BaseInitialise(UnitPlayer, Item.DrawSpeed, Item.KnockbackDistance);

                    switch (p.GetComponent<T>())
                    {
                        case Projectile:
                            p.GetComponent<T>().Initialise(UnitPlayer, _multiShotTargets[index],
                                Item.SticksOnWall, Item.ReturnsToPlayer, Item.IsRicochet,
                                Item.MaximumBounces);
                            AudioManager.audioManager.PlayWeapon(10);
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

        protected override bool CanUseProjectile()
        {
            if (targetCount <= 0) return false;

            return base.CanUseProjectile();
        }
    }
}