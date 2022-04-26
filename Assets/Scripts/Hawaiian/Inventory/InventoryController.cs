using System;
using System.Collections.Generic;
using Hawaiian.Input;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using Hawaiian.Unit;
using UnityEngine.InputSystem;


namespace Hawaiian.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [FormerlySerializedAs("canvas")] [SerializeField] private Transform uiParent;
        [SerializeField] private GameObject ui;
        [SerializeField] private Item currentItem;
        [SerializeField] private GameObject highlight;
        [SerializeField] private SpriteRenderer handheld;
        [SerializeField] private int invPosition = 0;
        [SerializeField] private bool addinv;
        [SerializeField] private GameEventListeners _listener;
        
        private GameObject reference;
        private Inventory _inv;
        private GameObject highRef;
        private PlayerAction play;
        private IUnit unit;
        private bool noDoubleDipping;

        #region ItemManagement

        private PlayerAction action;
        private PlayerInput _input;
        private UnitAnimator _animator;
        private Vector3 _lastAttackPosition;
        private Vector2 _rotation;
        private bool _attackFlag;
        private bool isLookingLeft;
        private bool _isJoystickNeutral = true;
        
                
        private bool _isHoldingAttack = false;
        private float _currentHoldTime;
        private float _offset = 1.1f;
        [SerializeField] private Transform _firePoint;
        
        [SerializeField] private Hawaiian.Input.Cursor _cursor;
        [SerializeField] private float _meleeSlashRotationOffset;
         private GameObject _projectileReference; //TODO: Get from item

         private LineRenderer _renderer;
         [SerializeField] private Vector2 _rendererPos1;
        #endregion

        public InventoryUI tempRef;

        public Item CurrentItem => GetCurrentItem();
        

        //[SerializeField] private int invSize;
        //[SerializeField] private Sprite hand;

        public Item GetCurrentItem()
        {
            return _inv.inv[invPosition];
        }
        
        private void Awake()
        {
            // TODO: Temp fix to get canvas reference. Will come back to this when we start looking at a UI system.
            if (uiParent == null) uiParent = FindObjectOfType<Canvas>().transform.GetChild(0);
            
            unit = GetComponent<IUnit>();
            _inv = ScriptableObject.CreateInstance<Inventory>();
            reference = Instantiate(ui, uiParent);
            tempRef = reference.GetComponent<InventoryUI>();
            reference.GetComponent<InventoryUI>().inv = _inv;
            noDoubleDipping = true;
            highRef = Instantiate(highlight, uiParent);
            play = new PlayerAction();
            _input = GetComponent<PlayerInput>();
            
            play.Player.Rotate.performed += ctx => UpdateRotation(ctx.ReadValue<Vector2>());
            play.Player.Rotate.canceled += ctx => CancelRotation();
            _renderer = GetComponent<LineRenderer>();
            play.Player.Attack.performed += OnAttack;
            play.Player.Attack.canceled += OnAttack;
        }

        public void InitialiseHighlight()
        {
            highRef.transform.position = tempRef.invSlots[invPosition].transform.position;
        }

        private void OnEnable()
        {
            play.Enable();
            _rotation = Vector2.zero;
        }
        

        #region ItemManagement

          void Update()
        {
            noDoubleDipping = true;
            if (addinv)
            {
                SelectionUpdate();
                addinv = false;
            }

            if (play.Player.InvParse.triggered)
                Parse((int) play.Player.InvParse.ReadValue<float>());

            if (_isHoldingAttack )
            {
                _renderer = BezierCurve.DrawQuadraticBezierCurve(_renderer,transform.localPosition,transform.localPosition + new Vector3(0.5f,2,0),_cursor.transform.position);

                if (Math.Abs(_cursor.CurrentRad - _cursor.MaxRadius) > 0.01f)
                {
                       
                    _currentHoldTime += Time.deltaTime;
                    UpdateHoldAttackCursor();
                }
                
            }
            _inv.PickUp(item);
            addinv = !addinv;
        }
        
        void UpdateRotation(Vector2 newValue)
        {
            _rotation = newValue;
            _isJoystickNeutral = false;
        }

        void UpdateHoldAttackCursor()
        {
            _cursor.CurrentRad += _currentHoldTime / 10;
        }

        void CancelRotation()
        {
            _rotation = Vector2.zero;
            _isJoystickNeutral = true;
        }

        #endregion

        private void OnDisable()
        {
            play.Disable();
        }
        
         public void HoldAttack(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                _isHoldingAttack = true;
                return;
            }

        

            var position = _cursor.transform.position;
            var instantiatedProjectile = Instantiate(_projectileReference,transform.position,Quaternion.identity);

            if (GetCurrentItem().Type == ItemType.Throwable)
            {

                List<Vector2> positions = new List<Vector2>();
                
                for (int i = 0; i < _renderer.positionCount; i++)
                    positions.Add((Vector2)_renderer.GetPosition(i));
                
                instantiatedProjectile.GetComponent<Throwable>().Initialise(positions.ToArray(),GetCurrentItem().ItemSprite,GetCurrentItem().DrawSpeed,GetCurrentItem().ItemDamage,GetCurrentItem().SticksOnWall);

            }
            else
                instantiatedProjectile.GetComponent<Projectile>().Initialise(position,GetCurrentItem().DrawSpeed,GetCurrentItem().ItemDamage,GetCurrentItem().SticksOnWall);

            
            _cursor.LerpToReset();
            _currentHoldTime = 0f;
            _isHoldingAttack = false;
        }
        
        
        public void OnAttack(InputAction.CallbackContext ctx)
        {

            if (GetCurrentItem() == null)
                return;
            
            //NEEDS TO BE A CHECK IF USING PROJECTILES TO ALLOW FOR ON HOLD ACTIONS AND IGNORE THIS
            #region Ranged Attack
            
            
            if (GetCurrentItem().Type is ItemType.Projectile or ItemType.Throwable)
            {
                HoldAttack(ctx);
                return;
            }
            
            
            #endregion

            #region MeleeAttack

            if (ctx.performed)
                return;
            
             Vector3 playerInput;
             float angle;
             
             var position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            // var mouse = UnityEngine.Input.mousePosition;
            
            
            
             if (_input.currentControlScheme == "Gamepad")
             {
            
                 if (_isJoystickNeutral)
                     _rotation = _animator.IsLookingLeft ? Vector2.left : Vector2.right;
                 
                 playerInput = _rotation;
                 _lastAttackPosition = position + (Vector3)playerInput * _offset;
                 angle = Mathf.Atan2(playerInput.y, playerInput.x) * Mathf.Rad2Deg;
            
             }
             else
             {
                 playerInput = UnityEngine.Input.mousePosition;
                 Vector3 worldPosition = Camera.main.ScreenToWorldPoint(playerInput);
                 worldPosition.z = 0f; // set to zero since its a 2d game
                 var mouseDirection = (worldPosition - position).normalized;
                 _lastAttackPosition = position + mouseDirection * _offset;
                 Vector3 difference = Camera.main.ScreenToWorldPoint(playerInput) -position;
                 difference.Normalize();
                 angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
             }
             
             
             _firePoint.position = _lastAttackPosition;
             GameObject indicator = Instantiate(_projectileReference,_lastAttackPosition,Quaternion.Euler(new Vector3(0,0,angle +_meleeSlashRotationOffset )),_firePoint);
            
             indicator.GetComponent<DamageIndicator>().Initialise(5,_attackFlag,unit);
             _attackFlag = !_attackFlag;

            #endregion
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        private void OnPickUp()
        {
            if (col.gameObject.tag == "Item" && noDoubleDipping)
            foreach (var target in positionalEventCaller.Targets)
            {
                noDoubleDipping = false;
                if(_inv.PickUp(col.gameObject.GetComponent<DroppedItem>().Item))
                {
                    Destroy(col.gameObject);
                    Debug.Log("Hit");
                    SelectionUpdate();
                }
                var item = target.GetComponent<DroppedItem>().item;
                
                if (item == null) continue;

                if (!_inv.PickUp(item)) continue;
                
                positionalEventCaller.Raise(target);
            }
        }

        private void Parse(int i)
        {
            invPosition += i;
            if (invPosition > _inv.inv.Length - 1)
            {
                invPosition = 0;
            }

            if (invPosition < 0)
            {
                invPosition = _inv.inv.Length - 1;
            }
            SelectionUpdate();
        }


        public void UpdateItem()
        {
            _projectileReference = GetCurrentItem().ProjectileInstance;
            handheld.sprite = GetCurrentItem().ItemSprite;
            _cursor.MaxRadius = GetCurrentItem().DrawDistance;
        }

        public void SelectionUpdate()
        {
            highRef.transform.position = tempRef.invSlots[invPosition].transform.position;
            handheld.sprite = _inv.inv[invPosition] != null ? _inv.inv[invPosition].ItemSprite : null;
            
            UpdateItem();
        }
    }
}
