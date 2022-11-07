using System.Linq;
using Hawaiian.Inventory.ItemBehaviours;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class InstantiateProjectileHeldItemBehaviour : InstantiateHeldItemBehaviour<Projectile>
    {
        public UnityEvent targetCountChanged = new();
        public UnityEvent multiShotTargetsUpdated = new();
        
        private bool collisionFlag = false;
        
        public Vector3[] _multiShotTargets;

        public override void Initialise(ItemHolder itemHolder)
        {
            base.Initialise(itemHolder);
            
            targetCount = Item.ProjectileAmount;
            
            targetCountChanged.Invoke();
            
            UpdateMultiShotTargets();
        }

        private int targetCount;

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

            for (var i = 0; i < targetCount; i++)
            {
                // Increment the angle for each target
                var currentAngle = angle + 20f * i - (targetCount - 1) / (float) 2 * 20f;

                var radians = currentAngle * Mathf.Deg2Rad;

                var x = Mathf.Cos(radians);
                var y = Mathf.Sin(radians);
                var targetPos = transform.position + new Vector3(x, y, 0f) * Cursor.CurrentRad;

                _multiShotTargets[i] = targetPos;
            }
            
            multiShotTargetsUpdated.Invoke();
        }

        protected override void InitialiseInstantiatedItemBehaviour(Projectile projectile, int i)
        {
            base.InitialiseInstantiatedItemBehaviour(projectile, i);
            
            projectile.Initialise(UnitPlayer, _multiShotTargets[i], Item);
        }

        protected override bool CanUseItem()
        {
            if (targetCount <= 0) return false;

            if (InstantiatedItemBehaviours.Any(projectile => projectile != null && !projectile.IsOnWall())) return false;
            
            if (Item.ProjectileAmount == 0) return false;

            return base.CanUseItem();
        }

        protected override int NumberOfObjectsToInstantiate()
        {
            return Item.ProjectileAmount;
        }
    }
}