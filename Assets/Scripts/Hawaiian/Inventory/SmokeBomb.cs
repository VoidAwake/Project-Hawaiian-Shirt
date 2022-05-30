using UnityEngine;

public class SmokeBomb : Throwable
{
   [Header("Smoke Bomb Settings")]
   [SerializeField] private GameObject _smokeEffectPrefab;
   

   public override void OnTargetReached()
   {
      Instantiate(_smokeEffectPrefab, LastPosition,Quaternion.identity);
      base.OnTargetReached();
      
   }
}
