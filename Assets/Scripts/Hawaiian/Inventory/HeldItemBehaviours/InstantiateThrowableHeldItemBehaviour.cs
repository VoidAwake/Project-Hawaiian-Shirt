using System.Collections.Generic;
using Hawaiian.Inventory.ItemBehaviours;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class InstantiateThrowableHeldItemBehaviour : InstantiateHeldItemBehaviour<Throwable>
    {
        [SerializeField] private IUnitGameEvent _removeItem;
        
        public UnityEvent throwableArcPositionsUpdated = new();
        
        public List<Vector2> throwableArcPositions = new List<Vector2>();

        // TODO: Move up
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

        protected override void InitialiseInstantiatedItemBehaviour(Throwable throwable, int i)
        {
            base.InitialiseInstantiatedItemBehaviour(throwable, i);
            
            throwable.Initialise(throwableArcPositions.ToArray(), Item.ItemSprite, Item.SticksOnWall);
            
            _removeItem.Raise(UnitPlayer);
            
            threw.Invoke();
        }
    }
}