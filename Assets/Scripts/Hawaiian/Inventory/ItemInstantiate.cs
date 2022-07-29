using System.Collections.Generic;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Inventory
{
    public class ItemInstantiate : MonoBehaviour
    {
        public Item item;
        private GameObject _projectileInstance;
        public bool _isHoldingAttack = false;
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
            
            heldItem.initialised.AddListener(OnInitialised);
            
            OnInitialised();
        }

        protected virtual void OnInitialised()
        {
            item = heldItem.Item;
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
            if (value.canceled)
            {
                Instantiate(value);
            }
            else
            {
                Aim();
            }
        }

        public void Aim()
        {
            if (!CanUseProjectile()) return;
            
            _isHoldingAttack = true;
        }

        public void Instantiate(InputAction.CallbackContext value)
        {
            if (!CanUseProjectile()) return;

            var projectiles = new List<GameObject>();

            for (int i = item.ProjectileAmount == 0 ? -1 : 0;
                 i < item.ProjectileAmount;
                 i++)
            {
                _projectileInstance = Instantiate(item.ProjectileInstance,
                    transform.position + 0.1f * (cursor.transform.position - transform.position), Quaternion.identity);
                projectiles.Add(_projectileInstance);

                if (i == -1)
                    break;
            }

            UseItem(projectiles);

            cursor.LerpToReset();
            _isHoldingAttack = false;
        }
        
        protected virtual bool CanUseProjectile()
        {
            if (_projectileInstance == null) return true;
            if (!_projectileInstance.GetComponent<Projectile>()) return true;
            if (_projectileInstance.GetComponent<Projectile>().IsOnWall()) return true;

            return false;
        }
        
        // TODO: Still need to have another look at these generics
        // private void UseItem<T>(List<GameObject> projectiles = null) where T : ItemBehaviour
        protected virtual void UseItem(List<GameObject> projectiles = null)
        {
        }
    }
}