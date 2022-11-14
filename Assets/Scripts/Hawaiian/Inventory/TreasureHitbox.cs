using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Cysharp.Threading.Tasks;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory
{
    [RequireComponent(typeof(Collider2D))]
    public class TreasureHitbox : ColliderGameObject
    {
        public event UnityAction DepositStarted;
        public event UnityAction<IUnit> CollidedUnit;
        
        public UnityEvent ItemDesposited;
        public CancellationTokenSource DepositToken;

        private bool _withinRadius;
        protected bool _isDepositing;
        
        protected override void RegisterCollisions()
        {
            OnTrigger.AddListener(_ => { _withinRadius = true; });
            OnTriggerExit.AddListener(_ => { _withinRadius = false; CancelDeposit();});
            OnCollision.AddListener(StartDeposit);
            OnCollisionExit.AddListener(_ => { CollidedUnit?.Invoke(null); });
        }

        protected override void UnRegisterCollisions()
        {
            OnTrigger.RemoveListener(_ => { _withinRadius = true; });
            OnTriggerExit.RemoveListener(_ =>
            {
                _withinRadius = false;
                CancelDeposit();
            });
            OnCollision.RemoveListener(StartDeposit);
            OnCollisionExit.RemoveListener(_ => { CollidedUnit?.Invoke(null); });
        }

        private void CancelDeposit() => DepositToken?.Cancel();


        private void StartDeposit(IUnit unit)
        {
            CollidedUnit?.Invoke(unit);
            DepositToken = new CancellationTokenSource();
            DepositStarted?.Invoke();
        }
    }
}