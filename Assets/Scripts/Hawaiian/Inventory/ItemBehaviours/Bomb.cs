using System.Collections.Generic;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using UnityEngine;

namespace Hawaiian.Inventory.ItemBehaviours
{
    public class Bomb : Throwable
    {
    
        [Header("Bomb Settings")]
        [SerializeField] private Dictionary<IUnit,Vector2> _knockbackDirection;
        [SerializeField] private GameObject _bombExplosionPrefab;


        public Dictionary<IUnit, Vector2> KnockbackDirections => _knockbackDirection;
    
    
        public override void OnTargetReached()
        { 
            GameObject bombEffect = Instantiate(_bombExplosionPrefab, LastPosition,Quaternion.identity);
            base.OnTargetReached();
        }
    }
}
