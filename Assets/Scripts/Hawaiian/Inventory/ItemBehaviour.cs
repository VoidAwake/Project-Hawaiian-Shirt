using System;
using Hawaiian.Unit;
using UnityEngine;

namespace Hawaiian.Inventory
{
    [Serializable]
    public abstract class ItemBehaviour : MonoBehaviour
    {
        [Header("Base Item Behaviour")]
        [SerializeField] internal float _speed;
        [SerializeField] internal int _knockbackDistance;

        public IUnit User;
        
        public virtual void BaseInitialise(IUnit user, float speed, int knockbackDistance)
        {
            User  = user;
            _speed = speed;
            _knockbackDistance = knockbackDistance;
        }
    }
}
