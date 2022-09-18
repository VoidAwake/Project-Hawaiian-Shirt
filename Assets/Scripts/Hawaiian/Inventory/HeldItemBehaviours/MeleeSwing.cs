using Hawaiian.Inventory.ItemBehaviours;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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

        public UnityEvent attacked = new();
            
        private float _slashCooldown;
        private Vector3 _lastAttackPosition;
        private bool _attackFlag;
        private float _offset = 1.1f;

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

            //Begin melee 
            Vector3 input = GetPlayerInput();
            var angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            var direction = input;
            InstantiateMeleeIndicator(angle, direction);
            
            attacked.Invoke();
        }

        private Vector3 GetPlayerInput()
        {
            _slashCooldown = AttackRate;
            Vector3 playerInput;

            var position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

            Vector3 prevInput = (Cursor.transform.localPosition - Vector3.up * 0.5f);
            playerInput = prevInput.magnitude == 0 ? Vector2.right.normalized : prevInput.normalized;

            _lastAttackPosition = position + playerInput * _offset + Vector3.down * 0.5f;
            return playerInput;
        }

        private void InstantiateMeleeIndicator(float angle, Vector3 direction)
        {
            FirePoint.position = _lastAttackPosition;

            UnitPlayer.transform.GetComponent<UnitAnimator>()
                .UseItem(UnitAnimationState.MeleeSwing,
                    new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad + Mathf.PI / 2),
                        -Mathf.Cos(angle * Mathf.Deg2Rad + Mathf.PI / 2)), _attackFlag);

            GameObject indicator = Instantiate(_projectileReference, _lastAttackPosition,
                Quaternion.Euler(new Vector3(0, 0, angle + _meleeSlashRotationOffset)), FirePoint);

            indicator.GetComponent<DamageIndicator>().Initialise(DrawSpeed, KnockbackDistance,
                _attackFlag, UnitPlayer, direction);
            indicator.GetComponent<HitUnit>().Initialise(UnitPlayer, Cursor.transform.position - transform.position);
            _attackFlag = !_attackFlag;
        }
    }
}