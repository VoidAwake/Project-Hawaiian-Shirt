using UnityEngine;

namespace Hawaiian.Inventory.ItemBehaviours
{
   // TODO: Duplicate code. See Bomb.
   public class SmokeBomb : Throwable
   {
      [Header("Smoke Bomb Settings")]
      [SerializeField] private GameObject _smokeEffectPrefab;
   

      public override void OnTargetReached()
      {
         Instantiate(_smokeEffectPrefab, transform.position, Quaternion.identity); // LastPosition,Quaternion.identity);
            base.OnTargetReached();
      
      }
   }
}
