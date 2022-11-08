using Hawaiian.Inventory.ItemBehaviours;
using Hawaiian.Inventory.ItemBehaviours.HitEffects;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class MeleeSwing : HeldItemBehaviour
    {
        // TODO: Get these from the item
        [SerializeField] private float _meleeSlashRotationOffset;
        [SerializeField] private GameObject _projectileReference;
        [SerializeField] private float AttackRate;
        [SerializeField] public float DrawSpeed;
        [SerializeField] public int KnockbackDistance;
        [SerializeField] private float _offset = 1.1f;

        public UnityEvent attacked = new();
            
        private float _slashCooldown;
        private Vector3 _lastAttackPosition;
        private bool _attackFlag;

        private float _minComboCooldown;
        private float _maxComboCooldown;
        private static int _maxCombo = 3;
        private int _currentCombo = 0;
        Coroutine resetComboCoroutine;

        public bool CanMeleeAttack() => _slashCooldown <= 0;

        public void Update()
        {
            if (_slashCooldown >= 0)
                _slashCooldown -= Time.deltaTime;
        }

        protected override void UseItemActionPerformed(InputAction.CallbackContext value)
        {
            base.UseItemActionPerformed(value);
            
            if (!CanMeleeAttack()) return;

            if (_currentCombo >= _maxCombo) return;

            _minComboCooldown = AttackRate * 1.65f;
            _maxComboCooldown = _minComboCooldown * 1.65f;

            //Begin melee 
            Vector3 input = GetPlayerInput();
            var angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            var direction = input;
            bool isFinalSwingOfCombo = _currentCombo >= _maxCombo - 1;
            InstantiateMeleeIndicator(angle, direction, isFinalSwingOfCombo);

            // Track combo of swings
            _currentCombo++;
            if (resetComboCoroutine != null)
                StopCoroutine(resetComboCoroutine);
            resetComboCoroutine = StartCoroutine(ResetCurrentCombo(isFinalSwingOfCombo ? _maxComboCooldown : _minComboCooldown));

            // Modify player's velocity
            UnitPlayer.SlowMovementTemporarily(AttackRate * 1.75f);
            UnitPlayer.BeginVelocityBurst((Vector2)direction, 0.3f, 0.075f); // I sure love magic numbers

            //print("Combo! " + currentCombo);

            attacked.Invoke();
        }

        private IEnumerator ResetCurrentCombo(float resetTime)
        {
            yield return new WaitForSeconds(resetTime);
            _currentCombo = 0;

            //print("Combo reset! " + currentCombo);
        }

        private Vector3 GetPlayerInput()
        {
            _slashCooldown = AttackRate;
            Vector3 playerInput;

            Vector3 prevInput = (Cursor.transform.localPosition - Vector3.up * 0.5f);
            playerInput = prevInput.magnitude == 0 ? Vector2.right.normalized : prevInput.normalized;

            _lastAttackPosition = transform.position + playerInput * _offset;
            return playerInput;
        }

        private void InstantiateMeleeIndicator(float angle, Vector3 direction, bool induceInvincibility = true)
        {
            UnitPlayer.transform.GetComponent<UnitAnimator>()
                .UseItem(UnitAnimationState.MeleeSwing,
                    new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad + Mathf.PI / 2),
                        -Mathf.Cos(angle * Mathf.Deg2Rad + Mathf.PI / 2)), _attackFlag);

            GameObject indicator = Instantiate(_projectileReference, _lastAttackPosition,
                Quaternion.Euler(new Vector3(0, 0, angle + _meleeSlashRotationOffset)), transform);

            indicator.GetComponent<DamageIndicator>().Initialise(DrawSpeed, KnockbackDistance,
                _attackFlag, UnitPlayer, direction);
            indicator.GetComponent<HitUnit>().Initialise(UnitPlayer, Cursor.transform.position - transform.position);
            _attackFlag = !_attackFlag;
            indicator.GetComponent<DealKnockback>().induceInvincibility = induceInvincibility;
        }
    }
}