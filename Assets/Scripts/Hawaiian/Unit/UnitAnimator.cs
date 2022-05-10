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
        None, MeleeSwing, Throw
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
        [SerializeField] private GameObject cursor;
        private const float itemUseSpeed = 0.28f;
        private float itemUseTimer;
        private Vector2 itemUseDirection;

        private Animator _animator;
        private Unit _unit;
        private bool _isLookingLeft;
        private Unit.PlayerState _playerState = Unit.PlayerState.Walking;
        private bool _wasInvincible = false;
        private float _invincibilityTimer;
        private bool isSwingingRight;

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

                        if (_currentAnimationState == UnitAnimationState.None)
                        {
                            FlipCharacter(true);
                        }
                    }
                    else if (spriteDirection > 0 && velocity.x < -0.1f)
                    {
                        //foreach (var renderer in _renderers)
                        //    renderer.flipX = false;

                        if (_currentAnimationState == UnitAnimationState.None)
                        {
                            FlipCharacter(false);
                        }
                    }
                }
                else if (_unit.playerState == Unit.PlayerState.Tripped)
                {
                    // Transition
                    if (_playerState != Unit.PlayerState.Tripped)
                    {
                        _animator.SetTrigger("trip");
                        _animator.speed = 1.0f;
                        _playerState = _unit.playerState;

                        if (_unit.isBeingHitRight())
                        {
                            //foreach (var renderer in _renderers)
                            //    renderer.flipX = false;

                            FlipCharacter(false);
                        }
                        else
                        {
                            //foreach (var renderer in _renderers)
                            //    renderer.flipX = true;

                            FlipCharacter(true);
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

                if (_invincibilityTimer > 0.1f)
                {
                    _invincibilityTimer %= 0.1f;

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

            if (_currentAnimationState == UnitAnimationState.MeleeSwing || _currentAnimationState == UnitAnimationState.Throw)
            {
                float swingBreadth = 1.0f; // Range of angles the swing will encompass
                float swingDepth = 1.0f; // Multiplier of the distance from the unit of the swing (radius of the swing circle, per say)
                if (_currentAnimationState == UnitAnimationState.MeleeSwing)
                {
                    swingBreadth = 2.2f * (isSwingingRight ? -1 : 1) * (transform.localScale.x < 0 ? -1 : 1);
                }
                else if (_currentAnimationState == UnitAnimationState.Throw)
                {
                    swingBreadth = 0.6f * (isSwingingRight ? -1 : 1);
                    swingDepth = 0.35f + 1.0f * Mathf.Sin((itemUseTimer / itemUseSpeed) * Mathf.PI);
                }

                float swingCurve = Mathf.Cos((itemUseTimer / itemUseSpeed) * Mathf.PI);
                float targetRadians = Mathf.Deg2Rad * Vector2.SignedAngle(Vector2.up, itemUseDirection);
                float percentage = 0.5f - 0.5f * Mathf.Pow(Mathf.Abs(swingCurve), 0.8f) * (swingCurve / Mathf.Abs(swingCurve));
                targetRadians += (swingBreadth / 2) - swingBreadth * percentage;

                // Set arm position and rotation
                Vector2 iDontKnowWhyThisWorksButItDoes = itemHand.transform.localPosition = new Vector2((0.5f * swingDepth) * Mathf.Sin(targetRadians) * (transform.localScale.x < 0 ? 1 : -1), 0.5f + (0.4f * swingDepth) * Mathf.Cos(targetRadians) * (transform.localScale.x < 0 ? -1 : 1));
                itemHand.transform.localPosition = new Vector2((0.5f * swingDepth) * Mathf.Sin(targetRadians) * (transform.localScale.x < 0 ? 1 : -1), 0.5f + (0.4f * swingDepth) * Mathf.Cos(targetRadians));
                itemHand.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Vector2.SignedAngle(Vector2.up, iDontKnowWhyThisWorksButItDoes + Vector2.down * 0.5f) - 30.0f);
                itemHand.transform.localScale = transform.localScale.x < 0 ? new Vector2(1.0f, -1.0f) : new Vector2(1.0f, 1.0f);

                // Set item position and rotation
                heldItem.transform.localPosition = new Vector2((0.5f * swingDepth + 0.4f) * Mathf.Sin(targetRadians) * (transform.localScale.x < 0 ? 1 : -1), 0.5f + (0.4f * swingDepth + 0.32f) * Mathf.Cos(targetRadians));
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

                    // Flip sprite if moving in opposite direction
                    if (_unit.GetVelocity().x > 0.1f)
                    {
                        //foreach (var renderer in _renderers)
                        //    renderer.flipX = true;

                        FlipCharacter(true);
                    }
                    else if (_unit.GetVelocity().x < -0.1f)
                    {
                        //foreach (var renderer in _renderers)
                        //    renderer.flipX = false;

                        FlipCharacter(false);
                    }
                }
            }

            #endregion
        }

        private void FlipCharacter(bool faceRightwards)
        {
            if (faceRightwards)
            {
                _isLookingLeft = false;
                spriteDirection = 1;
                //Vector2 prevPos = cursor.transform.position;
                transform.localScale = new Vector2(-1.0f, 1.0f);
                //if (cursor != null) cursor.transform.position = prevPos;
            }
            else
            {
                _isLookingLeft = true;
                spriteDirection = -1;
                //Vector2 prevPos = cursor.transform.position;
                transform.localScale = new Vector2(1.0f, 1.0f);
                //if (cursor != null) cursor.transform.position = prevPos;
            }
        }

        public void UseItem(UnitAnimationState animationState, Vector2 direction, bool rightToLeft)
        {
            _currentAnimationState = animationState;
            itemUseDirection = direction;
            itemUseTimer = 0.0f;
            itemHand.GetComponent<SpriteSkin>().enabled = false;
            isSwingingRight = rightToLeft;

            // Set character direciton based on direction of attack (this is put here instead of the initial item use transition because it messes up the melee projectile if the character is flipped too early)
            if (itemUseDirection.normalized.x > 0.1f)
            {
                FlipCharacter(true);
            }
            else if (itemUseDirection.normalized.x < -0.1f)
            {
                FlipCharacter(false);
            }
            // Is melee slash childed to this player? Flipping the player be kinda funky.
        }
    }
}
