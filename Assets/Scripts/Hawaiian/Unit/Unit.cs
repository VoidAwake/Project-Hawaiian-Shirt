using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Hawaiian.Input;
using UnityEngine.Events;

namespace Hawaiian.Unit
{
    public class Unit : UnitPhysics 
    {
        [SerializeField] bool testBool;
        [SerializeField] Vector2 testVector;
        [SerializeField] float testFloat;

        [SerializeField] SpriteResolver head;
        [SerializeField] SpriteResolver torso;

        enum TorsoLabels { Red, Blue, Yellow, Green }
        public enum HeadLabels { Fox, Robin, Monkey, Cat, Goose, Soup, Gambit, Bert }

        public enum PlayerState { Walking, Tripped, Struck }
        public PlayerState playerState = PlayerState.Walking;
        public bool OverrideDamage = false;


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
        private float remainingStruckTime;
        private const float struckTime = 0.2f;
        private bool wereControlsEnabledBeforeStruck = false;

        // Movement variables
        [Header("Movement Speed")]
        [SerializeField] internal float maxSpeed;
        [SerializeField] internal float tweenRate;
        [SerializeField] internal float runMultiplier;

        private bool knockbackOverride = false;
        protected Vector2 move = new Vector2(); // for directional input
        protected bool controlsEnabled = true;
        protected bool isRunning = false;
        internal bool isSlowed = false;
        public bool IsSlowed
        {
            get
            {
                return isSlowed;
            }
            set
            {
                isSlowed = value;
                if (temporarySlowCoroutine != null)
                    StopCoroutine(temporarySlowCoroutine);
            }
        }
        Coroutine temporarySlowCoroutine;

        bool isBurstActive;
        float burstTimer;
        float burstDuration;
        Vector2 burstForce;

        public UnityEvent initialised = new();
        public UnityEvent getStruckEvent = new();

        protected virtual void Start()
        {
            _health = _maxHealth;
        }

        public void BeginVelocityBurst(Vector2 direction, float distance, float duration) // Overrides walking velocity, used for melee slashes
        {
            isBurstActive = true;
            burstTimer = 0.0f;
            burstDuration = duration;
            burstForce = direction.normalized * distance / duration * 2.0f; // Burst force diminishses at a linear rate over the duration, hence area under curve is half and we times by 2
        }

        protected override void SetTargetVelocity()
        {
            // Debug update code
            if (testBool)
            {
                //print(name + "... " + "Test?! State: " + playerState + ". isBurstActive: " + isBurstActive + ". knockBackOverride: " + knockbackOverride + ".");

                testBool = false;
                GetComponent<UnitAnimator>().UseItem(UnitAnimationState.Throw, testVector, false);
            }

            if (playerState == PlayerState.Struck)
            {
                //print(name + "... " + "Struck. State: " + playerState + ". isBurstActive: " + isBurstActive + ". knockBackOverride: " + knockbackOverride + ".");

                remainingStruckTime -= Time.deltaTime;
                if (remainingStruckTime <= 0.0f)
                {
                    remainingStruckTime = 0.0f;
                    playerState = PlayerState.Tripped;
                    controlsEnabled = wereControlsEnabledBeforeStruck;
                }

                ModifyVelocityBasedOnBurst();
            }
            if (playerState == PlayerState.Walking && !knockbackOverride)
            {
                //print(name + "... " + "Walkin' n' fine. State: " + playerState + ". isBurstActive: " + isBurstActive + ". knockBackOverride: " + knockbackOverride + ".");

                //bool debug = (move.magnitude > 0.1f);
                //if (debug) print("Walking and fine. isBurstActive: " + isBurstActive + ". Velocity: " + velocity + ". Move input: " + move + ". isSlowed: " + isSlowed + ". Control enabled: " + controlsEnabled);

                // Update inputs and velocity
                Vector2 modifiedMove = move.magnitude * 1.2f > 1.0f ? move.normalized : move.magnitude < 0.05f ? Vector2.zero : move * 1.2f;
                if (isSlowed)
                    modifiedMove *= 0.2f;
                if (controlsEnabled) velocity = Vector2.Lerp(velocity, maxSpeed * (isRunning ? runMultiplier * modifiedMove : modifiedMove), Mathf.Clamp(Time.deltaTime * gameTimeScale.Value * tweenRate, 0.0f, 1.0f));
                else velocity = Vector2.Lerp(velocity, Vector2.zero, Mathf.Clamp(Time.deltaTime * gameTimeScale.Value * tweenRate, 0.0f, 1.0f));

                ModifyVelocityBasedOnBurst();
            }
            else if (playerState == PlayerState.Tripped || knockbackOverride)
            {
                //print(name + "... " + "Tripped or knocked. State: " + playerState + ". isBurstActive: " + isBurstActive + ". knockBackOverride: " + knockbackOverride + ".");

                // Set velocity based on knockback curve
                velocity = knockBackForce * 2 * (remainingTripTime / tripTime);
                //Debug.Log("Knockback!");

                // Update timer, and stand back up if trip time has expired
                remainingTripTime -= Time.deltaTime;
                if (remainingTripTime < 0.0f)
                {
                    remainingTripTime = 0.0f;
                    knockbackOverride = false;
                    playerState = PlayerState.Walking;
                }
            }
            else
            {
                //print(name + "... " + "End of the line. State: " + playerState + ". isBurstActive: " + isBurstActive + ". knockBackOverride: " + knockbackOverride + ".");
                velocity = Vector2.Lerp(velocity, Vector2.zero, Mathf.Clamp(Time.deltaTime * gameTimeScale.Value * tweenRate, 0.0f, 1.0f));
            }
        }

        private void ModifyVelocityBasedOnBurst()
        {
            if (isBurstActive)
            {
                velocity = Vector2.Lerp(burstForce, velocity, burstTimer / burstDuration);
                burstTimer += Time.deltaTime;
                if (burstTimer > burstDuration)
                    isBurstActive = false;
            }
        }

        public void SlowMovementTemporarily(float duration)
        {
            IsSlowed = true;
            temporarySlowCoroutine = StartCoroutine(ResetIsSlowed(duration));
        }

        IEnumerator ResetIsSlowed(float duration)
        {
            yield return new WaitForSeconds(duration);
            IsSlowed = false;
        }

        private void GetStruck(float newStruckTime)
        {
            //print("Get struck! Struck time is " + newStruckTime + "usually is " + struckTime);
            getStruckEvent.Invoke();

            if (playerState != PlayerState.Struck)
                wereControlsEnabledBeforeStruck = controlsEnabled;
            controlsEnabled = false;

            playerState = PlayerState.Struck;
            remainingStruckTime = newStruckTime;
        }

        public void KnockBack(Vector2 direction, float distance, bool induceInvincibility = true)
        {
            if (isInvincible) return;

            // Talk shit
            knockBackForce = direction.normalized * distance;

            // Get hit
            GetStruck(induceInvincibility ? struckTime : struckTime * 1.65f); // Greater delay if hit by a weapon that can combo 
            BeginVelocityBurst(direction, 0.3f, 0.05f);
            remainingTripTime = tripTime;

            if (induceInvincibility)
                BecomeInvincible(invincibilityTime);
        }

        public void ApplyKnockbackOnly(Vector2 direction, float distance, float duration = 0.5f)
        {
            print("Apply knockback only!");
            // Talk shit
            knockBackForce = direction.normalized * distance;

            // Get hit
            knockbackOverride = true;
            remainingTripTime = duration;   
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

        public void SetSpriteResolvers(string headName, string torsoName)
        {
            head.SetCategoryAndLabel("Head", headName);
            torso.SetCategoryAndLabel("Torso", torsoName);
        }

        public virtual void Initialise(int characterNumber, int playerNumber)
        {
            SetSpriteResolvers(((HeadLabels) characterNumber).ToString(), ((TorsoLabels) playerNumber).ToString());
            
            initialised.Invoke();
        }


    }
}
