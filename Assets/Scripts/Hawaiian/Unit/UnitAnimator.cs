using System;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Hawaiian.Utilities;
using Hawaiian.Unit;

namespace Hawaiian.Unit
{
    public enum UnitAnimationState
    {
        //Idle = 0,
        //Walking = 2,
        //Attacking = 4
        None, MeleeSwing
    }
        
    [RequireComponent(typeof(Unit))][RequireComponent(typeof(Animator))]
    public class UnitAnimator : MonoBehaviour
    {
        private int spriteDirection = -1;
        [SerializeField] private SpriteRenderer[] _renderers;
        [SerializeField] private ScriptableFloat gameTimeScale;
        [SerializeField] private UnitAnimationState _currentAnimationState;
        [SerializeField] private GameObject itemHand;
        [SerializeField] private GameObject heldItem;
        private const float itemUseSpeed = 0.2f;
        private float itemUseTimer;
        private Vector2 itemUseDirection;

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
            #region Player State Animation

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

            #endregion

            #region Invincibility Flashing

            if (_unit.isInvincible)
            {
                _invincibilityTimer += Time.deltaTime * gameTimeScale.Value;
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

            #endregion

            #region Use Item / Hands Override

            if (_currentAnimationState == UnitAnimationState.MeleeSwing)
            {
                float targetRadians = Mathf.Deg2Rad * Vector2.SignedAngle(Vector2.up, itemUseDirection);
                float altRadians = targetRadians + (-1.6f - 3.2f * (itemUseTimer / itemUseSpeed));
                targetRadians += -1.6f + 3.2f * (itemUseTimer / itemUseSpeed);

                // Set arm position and rotation
                itemHand.transform.localPosition = new Vector2(0.65f * Mathf.Sin(targetRadians) * (transform.localScale.x < 0 ? 1 : -1), 0.5f + 0.5f * Mathf.Cos(targetRadians));
                if (transform.localScale.x < 0)
                {
                    Vector2 altPosition = new Vector2(0.65f * Mathf.Sin(altRadians) * (transform.localScale.x < 0 ? 1 : -1), 0.5f + 0.5f * Mathf.Cos(altRadians));
                    itemHand.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Vector2.SignedAngle(Vector2.up, altPosition + Vector2.down * 0.5f));
                }
                else
                {
                    itemHand.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Vector2.SignedAngle(Vector2.up, (Vector2)itemHand.transform.localPosition + Vector2.down * 0.5f));
                }
                itemHand.transform.localScale = transform.localScale.x < 0 ? new Vector2(-1.0f, 1.0f) : new Vector2(1.0f, 1.0f);

                // Set item position and rotation
                heldItem.transform.localPosition = new Vector2(0.95f * Mathf.Sin(targetRadians) * (transform.localScale.x < 0 ? 1 : -1), 0.5f + 0.7f * Mathf.Cos(targetRadians));
                heldItem.transform.rotation = itemHand.transform.rotation;
                heldItem.transform.localScale = itemHand.transform.localScale;

                // Increment timer and manage state exit transition
                itemUseTimer += Time.deltaTime * gameTimeScale.Value;

                if (itemUseTimer > itemUseSpeed)
                {
                    _currentAnimationState = UnitAnimationState.None;
                    itemUseTimer = 0.0f;
                    itemHand.GetComponent<SpriteSkin>().enabled = true;

                    heldItem.transform.localPosition = new Vector2(-0.6f, 0.55f);
                    heldItem.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    heldItem.transform.localScale = new Vector2(1.0f, 1.0f);
                    itemHand.transform.localScale = new Vector2(1.0f, 1.0f);
                }
            }

            #endregion
        }

        public void UseItem(UnitAnimationState animationState, Vector2 direction)
        {
            _currentAnimationState = animationState;
            itemUseDirection = direction;
            itemUseTimer = 0.0f;
            itemHand.GetComponent<SpriteSkin>().enabled = false;
        }
    }
}
