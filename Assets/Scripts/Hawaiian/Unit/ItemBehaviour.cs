using System;
using UnityEngine;
using System.Collections.Generic;
namespace Hawaiian.Unit
{
    
    [Serializable]
    public abstract class ItemBehaviour : MonoBehaviour
    {
        [Header("Base Item Behaviour")]
        [SerializeField] internal float _speed;
        [SerializeField] internal int _knockbackDistance;

        internal IUnit _user;

        public virtual void BaseInitialise(IUnit user, float speed, int knockbackDistance)
        {
            _user = user;
            _speed = speed;
            _knockbackDistance = knockbackDistance;
        }
        
      //  public virtual void Initialise(float parryWindow, int parryPercentage, SpriteRenderer spriteReference, Sprite[] shieldStates, GameE){}

        public virtual void Initialise(Vector2[] targets, Sprite newSprite, bool canStickOnWalls = false) {}

        public virtual void Initialise(IUnit user, Vector3 target, bool canStickOnWalls = false, bool returnsToPlayer = false, bool ricochet = false, int maximumBounce = 0){}
        
        public virtual void Initialise(float speed, int knockbackDistance,  bool flag, IUnit user, Vector2 knockbackDirection) {}

    }
}
