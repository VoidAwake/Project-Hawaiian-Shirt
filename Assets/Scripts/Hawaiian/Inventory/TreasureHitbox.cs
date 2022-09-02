using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Cysharp.Threading.Tasks;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory
{
    public class TreasureHitbox : ColliderGameObject
    {

        protected CancellationTokenSource _depositToken;

        protected bool _withinRadius;
        protected bool _isDepositing;

        protected override void RegisterCollisions()
        {
            OnTrigger.AddListener(unit => { _withinRadius = true; });
            OnTriggerExit.AddListener(unit => { _withinRadius = false; });
            OnCollision.AddListener(StartDeposit);
        }

        protected override void UnRegisterCollisions()
        {
            OnTrigger.RemoveListener(unit => { _withinRadius = true; });
            OnTriggerExit.RemoveListener(unit => { _withinRadius = false; });
            OnCollision.RemoveListener(StartDeposit);
        }


        private async void StartDeposit(IUnit unit)
        {
            _depositToken = new CancellationTokenSource();

         //   if (!CanDeposit(unit))
           //     return;

        }

   

      
    }
}