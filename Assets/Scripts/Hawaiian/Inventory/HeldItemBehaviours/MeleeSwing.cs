using Hawaiian.Inventory.ItemBehaviours;
using Hawaiian.Unit;
using UnityEngine;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class MeleeSwing : MonoBehaviour
    {
        [SerializeField] private float _meleeSlashRotationOffset;
        [SerializeField] private GameObject _projectileReference;
        [SerializeField] private float AttackRate;
        [SerializeField] public float DrawSpeed;
        [SerializeField] public int KnockbackDistance;
            
        private float _slashCooldown;
        private Vector3 _lastAttackPosition;
        private bool _attackFlag;
        private float _offset = 1.1f;
        private UnitPlayer _playerReference;
        private Cursor cursor;
        private Transform firePoint;

        public bool CanMeleeAttack() => _slashCooldown <= 0;

        private void Awake()
        {
            // TODO: Need to consider where we're getting these references
            _playerReference = GetComponentInParent<ItemHolder>().unitPlayer;
            cursor = GetComponentInParent<ItemHolder>().cursor;
            firePoint = GetComponentInParent<ItemHolder>().firePoint;
        }

        public void Update()
        {
            if (_slashCooldown >= 0)
                _slashCooldown -= Time.deltaTime;
        }

        public void Swing()
        {
            if (!CanMeleeAttack()) return;

            //Begin melee 
            Vector3 input = GetPlayerInput();
            AudioManager.audioManager.PlayWeapon(7);
            var angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            var direction = input;
            InstantiateMeleeIndicator(angle, direction);
        }

        private Vector3 GetPlayerInput()
        {
            _slashCooldown = AttackRate;
            Vector3 playerInput;

            var position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

            Vector3 prevInput = (cursor.transform.localPosition - Vector3.up * 0.5f);
            playerInput = prevInput.magnitude == 0 ? Vector2.right.normalized : prevInput.normalized;

            _lastAttackPosition = position + playerInput * _offset;
            return playerInput;
        }

        private void InstantiateMeleeIndicator(float angle, Vector3 direction)
        {
            firePoint.position = _lastAttackPosition;

            _playerReference.transform.GetComponent<UnitAnimator>()
                .UseItem(UnitAnimationState.MeleeSwing,
                    new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad + Mathf.PI / 2),
                        -Mathf.Cos(angle * Mathf.Deg2Rad + Mathf.PI / 2)), _attackFlag);

            GameObject indicator = Instantiate(_projectileReference, _lastAttackPosition,
                Quaternion.Euler(new Vector3(0, 0, angle + _meleeSlashRotationOffset)), firePoint);

            indicator.GetComponent<DamageIndicator>().Initialise(DrawSpeed, KnockbackDistance,
                _attackFlag, _playerReference, direction);
            indicator.GetComponent<HitUnit>().Initialise(_playerReference, cursor.transform.position - transform.position);
            _attackFlag = !_attackFlag;
        }
    }
}