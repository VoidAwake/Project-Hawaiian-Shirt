using System.Collections.Generic;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory
{
    public abstract class ColliderGameObject : MonoBehaviour
    {
        internal readonly UnityEvent<IUnit> OnTrigger = new();
        internal readonly UnityEvent<IUnit> OnTriggerExit  = new();
        internal readonly UnityEvent<IUnit> OnCollision = new();
        internal readonly UnityEvent<IUnit> OnCollisionExit  = new();

        protected IList<IUnit> OnCollidedPlayers = new List<IUnit>();
        protected IList<IUnit> OnTriggeredPlayers = new List<IUnit>();

        protected virtual void OnEnable()
        {
            RegisterCollisions();
        }

        protected virtual void OnDisable()
        {
            UnRegisterCollisions();
        }


        private void OnTriggerEnter2D(Collider2D col)
        {
            IUnit collidedUnit;

            if (col.gameObject.TryGetComponent<IUnit>(out collidedUnit))
            {
                OnTriggeredPlayers.Add(collidedUnit);
                OnTrigger.Invoke(collidedUnit);
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            IUnit collidedUnit;

            if (col.gameObject.TryGetComponent<IUnit>(out collidedUnit))
            {
                if (OnTriggeredPlayers.Contains(collidedUnit))
                    OnTriggeredPlayers.Remove(collidedUnit);

                OnTriggerExit.Invoke(collidedUnit);
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            IUnit collidedUnit;

            if (col.gameObject.TryGetComponent<IUnit>(out collidedUnit))
            {
                OnCollidedPlayers.Add(collidedUnit);
                OnCollision.Invoke(collidedUnit);
            }
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            IUnit collidedUnit;

            if (col.gameObject.TryGetComponent<IUnit>(out collidedUnit))
            {
                if (OnCollidedPlayers.Contains(collidedUnit))
                    OnCollidedPlayers.Remove(collidedUnit);

                OnCollisionExit.Invoke(collidedUnit);
            }
        }

        protected abstract void RegisterCollisions();

        protected abstract void UnRegisterCollisions();
    }
}