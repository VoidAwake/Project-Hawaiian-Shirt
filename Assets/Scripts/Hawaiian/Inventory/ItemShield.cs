using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Inventory
{
    // TODO: Merge with Shield?
    public class ItemShield : MonoBehaviour
    {
        [SerializeField] private IUnitGameEvent _parryOccured;
        [SerializeField] private GameObject shieldColliderPrefab;
        [SerializeField] private Shield _shieldReference;
        private SpriteRenderer heldItemSpriteRenderer;

        // TODO: Duplicate code. See ItemInteractor
        public Item item;
        public UnitPlayer _playerReference;
        protected Cursor cursor;
        private Transform firePoint;
        private HeldItem heldItem;
        
        private void Awake()
        {
            // TODO: Need to consider where we're getting these references
            _playerReference = GetComponentInParent<ItemHolder>().unitPlayer;
            cursor = GetComponentInParent<ItemHolder>().cursor;
            firePoint = GetComponentInParent<ItemHolder>().firePoint;
            heldItem = GetComponent<HeldItem>();
            heldItemSpriteRenderer = GetComponentInParent<ItemHolder>().heldItemSpriteRenderer;

            heldItem.initialised.AddListener(OnInitialised);
            
            OnInitialised();
        }
        protected virtual void OnEnable()
        {
            _playerReference.GetPlayerInput().actions["Attack"].performed += StartAttack;
            _playerReference.GetPlayerInput().actions["Attack"].canceled += StartAttack;
        }
        
        protected virtual void OnDisable()
        {
            _playerReference.GetPlayerInput().actions["Attack"].performed -= StartAttack;
            _playerReference.GetPlayerInput().actions["Attack"].canceled -= StartAttack;
        }
        public void StartAttack(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                Attack();
            }
        }

        protected virtual void OnInitialised()
        {
            item = heldItem.Item;
            
            _shieldReference.Initialise(item.ParryWindow,
                new[] { item.ParryPercentageUpperLimit, item.ParryPercentageLowerLimit }, heldItemSpriteRenderer,
                new[] { item.ShieldDown, item.ShieldUp }, _parryOccured, shieldColliderPrefab,
                item.TimeTillParry, _playerReference, cursor.transform);
        }

        public void Attack()
        {
            if (_shieldReference.CanParry())
                _shieldReference.LiftShield();
        }
    }
}