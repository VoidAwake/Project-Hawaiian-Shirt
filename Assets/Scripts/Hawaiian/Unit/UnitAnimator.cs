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
        private int spriteDirection = -1;

        [SerializeField] private UnitAnimationState _currentAnimationState;
        
        private Animator _animator;
        private Unit _unit;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _unit = GetComponent<Unit>();
        }

        private void Update()
        {
            if (_animator != null && _unit != null)
            {
                // Set animator parameters
                Vector2 velocity = _unit.GetVelocity();
                _animator.SetFloat("speed", velocity.magnitude);
                _animator.SetBool("isRunning", _unit.GetIsFullSpeed());
                if (_unit.GetIsFullSpeed())
                {
                    _animator.speed = velocity.magnitude;
                }
                else
                {
                    _animator.speed = 1.0f;
                }

                // Flip sprite if moving in opposite direction
                if (spriteDirection < 0 && velocity.x > 0.1f)
                {
                    spriteDirection = 1;
                    transform.localScale = new Vector2(-1.0f, 1.0f);
                }
                else if (spriteDirection > 0 && velocity.x < -0.1f)
                {
                    spriteDirection = -1;
                    transform.localScale = new Vector2(1.0f, 1.0f);
                }
            }
        }

    }
}
