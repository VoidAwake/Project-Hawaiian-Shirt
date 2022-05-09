using System.Collections;
using UnityEngine;

namespace Hawaiian.Unit
{
    public class Unit : UnitPhysics 
    {
        [SerializeField] bool testBool;
        [SerializeField] Vector2 testVector;
        [SerializeField] float testFloat;

        public enum PlayerState { Walking, Tripped }
        public PlayerState playerState = PlayerState.Walking;

        [Header("Unit Stats")]
        [SerializeField]internal float _maxHealth;

        internal float _health;    
        public float Health => _health;

        // Tripped/Hit variables
        [SerializeField] internal float tripTime = 0.75f;
        [SerializeField] internal float invincibilityTime = 1.0f;
        public bool isInvincible = false;
        internal Coroutine invincibilityCoroutine;
        internal float remainingTripTime;
        internal Vector2 knockBackForce;

        // Movement variables
        [Header("Movement Speed")]
        [SerializeField] internal float maxSpeed;
        [SerializeField] internal float tweenRate;
        [SerializeField] internal float runMultiplier;

        protected Vector2 move = new Vector2(); // for directional input
        protected bool controlsEnabled = true;
        protected bool isRunning = false;

        protected virtual void Start()
        {
            _health = _maxHealth;
        }

        protected override void SetTargetVelocity()
        {
            // Debug update code
            if (testBool)
            {
                testBool = false;
                GetComponent<UnitAnimator>().UseItem(UnitAnimationState.Throw, testVector, false);
            }

            if (playerState == PlayerState.Walking)
            {
                // Update inputs and velocity
                Vector2 modifiedMove = move.magnitude * 1.2f > 1.0f ? move.normalized : move.magnitude < 0.05f ? Vector2.zero : move * 1.2f;
                if (controlsEnabled) velocity = Vector2.Lerp(velocity, maxSpeed * (isRunning ? runMultiplier * modifiedMove : modifiedMove), Mathf.Clamp(Time.deltaTime * gameTimeScale.Value * tweenRate, 0.0f, 1.0f));
                else velocity = Vector2.Lerp(velocity, Vector2.zero, Mathf.Clamp(Time.deltaTime * gameTimeScale.Value * tweenRate, 0.0f, 1.0f));
            }
            else if (playerState == PlayerState.Tripped)
            {
                // Set velocity based on knockback curve
                velocity = knockBackForce * 2 * (remainingTripTime / tripTime);
                Debug.Log("Knockback!");

                // Update timer, and stand back up if trip time has expired
                remainingTripTime -= Time.deltaTime;
                if (remainingTripTime < 0.0f)
                {
                    remainingTripTime = 0.0f;
                    playerState = PlayerState.Walking;
                }
            }
            else
            {
                velocity = Vector2.Lerp(velocity, Vector2.zero, Mathf.Clamp(Time.deltaTime * gameTimeScale.Value * tweenRate, 0.0f, 1.0f));
            }
        }

        public void KnockBack(Vector2 direction, float distance)
        {
            // Talk shit
            knockBackForce = direction.normalized * distance;

            // Get hit
            playerState = PlayerState.Tripped;
            remainingTripTime = tripTime;
            BecomeInvincible(invincibilityTime);
        }

        public void BecomeInvincible(float duration)
        {
            if (isInvincible)
            {
                StopCoroutine(invincibilityCoroutine);
            }
            else
            {
                isInvincible = true;
            }

            invincibilityCoroutine = StartCoroutine(ResetInvincibility(invincibilityTime));
        }

        IEnumerator ResetInvincibility(float duration)
        {
            // Wait invinvibility period
            yield return new WaitForSeconds(duration);

            // Then turn off invincibility
            isInvincible = false;
        }

        // Following functions are used by the Unit Animator component:

        public Vector2 GetVelocity()
        {
            return velocity;
        }

        public bool GetIsFullSpeed()
        {
            return isRunning && move.magnitude > 0.1f || move.magnitude >= 0.5f;
        }
     
        public bool isBeingHitRight()
        {
            if (knockBackForce.x > 0.0f) return true;
            else return false;
        }
    }
}
