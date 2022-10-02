using Hawaiian.Inventory.ItemBehaviours;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class ItemShoot : ItemInstantiate<Projectile>
    {
        public UnityEvent targetCountChanged = new();
        public UnityEvent multiShotTargetsUpdated = new();
        
        private bool collisionFlag = false;
        
        public Vector3[] _multiShotTargets;
        private int targetCount;

        public UnityEvent shot = new();

        public override void Initialise(ItemHolder itemHolder)
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

        protected override void InitialiseInstantiatedItemBehaviour(Projectile projectile, int i)
        {
            base.InitialiseInstantiatedItemBehaviour(projectile, i);
            
            projectile.Initialise(UnitPlayer, _multiShotTargets[i], Item);
                
            shot.Invoke();
        }

        protected override bool CanUseProjectile()
        {
            if (targetCount <= 0) return false;

            return base.CanUseProjectile();
        }
    }
}