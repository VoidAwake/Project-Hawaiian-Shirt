using System.Collections.Generic;
using Hawaiian.Unit;
using UnityEngine;

public class Bomb : Throwable
{
    
    [Header("Bomb Settings")]
    [SerializeField] private Dictionary<IUnit,Vector2> _knockbackDirection;
    [SerializeField] private GameObject _bombExplosionPrefab;


    public Dictionary<IUnit, Vector2> KnockbackDirections => _knockbackDirection;
    
    
    public override void OnTargetReached()
    { 
       GameObject bombEffect =  Instantiate(_bombExplosionPrefab, LastPosition,Quaternion.identity);
        AudioManager.audioManager.PlayWeapon(8);
        base.OnTargetReached();
      
    }
}
