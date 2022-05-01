using System;
using UnityEngine;
using Hawaiian.Unit;

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
       [SerializeField] private SpriteRenderer[] _renderers;
       
        [SerializeField] private UnitAnimationState _currentAnimationState;
        
        private Animator _animator;
        private Unit _unit;
        private bool _isLookingLeft;
        private Unit.PlayerState _playerState = Unit.PlayerState.Walking;
        private bool _wasInvincible = false;
        private float _invincibilityTimer;


        public bool IsLookingLeft => _isLookingLeft;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _unit = GetComponent<Unit>();
            _isLookingLeft = true;
        }

        private void Update()
        {
            if (_animator != null && _unit != null)
            {
                if (_unit.playerState == Unit.PlayerState.Walking)
                {
                    // Transition
                    if (_playerState != Unit.PlayerState.Walking)
                    {
                        _animator.SetTrigger("walk");
                        _playerState = _unit.playerState;
                    }

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
                        //foreach (var renderer in _renderers)
                        //    renderer.flipX = true;

                        _isLookingLeft = false;
                        spriteDirection = 1;
                        transform.localScale = new Vector2(-1.0f, 1.0f);
                    }
                    else if (spriteDirection > 0 && velocity.x < -0.1f)
                    {
                        //foreach (var renderer in _renderers)
                        //    renderer.flipX = false;

                        _isLookingLeft = true;
                        spriteDirection = -1;
                        transform.localScale = new Vector2(1.0f, 1.0f);
                    }
                }
                else if (_unit.playerState == Unit.PlayerState.Tripped)
                {
                    // Transition
                    if (_playerState != Unit.PlayerState.Tripped)
                    {
                        _animator.SetTrigger("trip");
                        _playerState = _unit.playerState;

                        if (_unit.isBeingHitRight())
                        {
                            //foreach (var renderer in _renderers)
                            //    renderer.flipX = false;

                            _isLookingLeft = true;
                            spriteDirection = -1;
                            transform.localScale = new Vector2(1.0f, 1.0f);
                        }
                        else
                        {
                            //foreach (var renderer in _renderers)
                            //    renderer.flipX = true;

                            _isLookingLeft = false;
                            spriteDirection = 1;
                            transform.localScale = new Vector2(-1.0f, 1.0f);
                        }
                    }
                }
            }

            if (_unit.isInvincible)
            {
                _invincibilityTimer += Time.deltaTime;
                _wasInvincible = true;

                if (_invincibilityTimer > 0.075f)
                {
                    _invincibilityTimer %= 0.075f;

                    // Loop through children and enable/disable any sprites
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        SpriteRenderer child = transform.GetChild(i).GetComponent<SpriteRenderer>();
                        if (child != null)
                        {
                            if (child.enabled) child.enabled = false;
                            else child.enabled = true;
                        }
                    }
                }
            }
            else if (_wasInvincible)
            {
                _invincibilityTimer = 0.0f;
                _wasInvincible = false;

                // Loop through children and enable any sprites
                for (int i = 0; i < transform.childCount; i++)
                {
                    SpriteRenderer child = transform.GetChild(i).GetComponent<SpriteRenderer>();
                    if (child != null)
                    {
                        child.enabled = true;
                    }
                }
            }
        }

    }
}
