using System;
using UnityEngine;

namespace Hawaiian.Unit
{
    public enum UnitAnimationState
    {
        Idle = 0,
        Walking = 2,
        Attacking = 4
    }
    
    
    [RequireComponent(typeof(Unit))][RequireComponent(typeof(Animator))]
    public class UnitAnimator : MonoBehaviour
    {
        
        [SerializeField] private UnitAnimationState _currentAnimationState;
        
        private Animator _animator;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

    }
}
