using System.Collections;
using System.Collections.Generic;
using Hawaiian.Inventory;
using Hawaiian.Utilities;
using UnityEngine;

public class SmokeBomb : Throwable
{
   [Header("Smoke Bomb Settings")]
   [SerializeField] private GameObject _smokeEffectPrefab;
   
   [SerializeField] private GameEvent _event;

   public override void OnTargetReached()
   {
      Instantiate(_smokeEffectPrefab, LastPosition,Quaternion.identity);
      _event.Raise();
      base.OnTargetReached();
      
   }
}