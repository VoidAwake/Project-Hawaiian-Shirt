using System;
using System.Collections.Generic;
using System.Dynamic;
using Hawaiian.Inventory;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using Hawaiian.Input;


[RequireComponent(typeof(InventoryController))]
public class ItemInteractor : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private LineRenderer _renderer;
    [SerializeField] private Hawaiian.Input.Cursor _cursor;
    [SerializeField] private UnitPlayer _playerReference;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _meleeSlashRotationOffset;
    [SerializeField] private SpriteRenderer _handHelder;
    
    //Components
    private InventoryController _controller;
    
    
    private Vector2 _rotation;

   [SerializeField] private UnitAnimator _animator;
    private Vector3 _lastAttackPosition;
    private bool _attackFlag;
    private bool isLookingLeft;
    private bool _isJoystickNeutral = true;
    private bool _isHoldingAttack = false;
    private float _currentHoldTime;
    private float _offset = 1.1f;
    
    private GameObject _projectileReference; //TODO: Get from item


    
    public Vector2 Rotation
    {
        get => _rotation;
        set => _rotation = value;
    }
    private void Awake()
    {
        _controller = GetComponent<InventoryController>();
        _renderer = GetComponent<LineRenderer>();
        _playerReference.GetPlayerInput().actions["Attack"].performed += StartAttack;
        _playerReference.GetPlayerInput().actions["Attack"].canceled += StartAttack;
    }

    public void Start()
    {
        if (_playerReference != null)
        {

            
           // _playerReference.GetPlayerAction().Player.Rotate.canceled += ctx => CancelRotation();

           // _playerReference.GetPlayerAction().Player.Attack.performed += OnAttack;
           // _playerReference.GetPlayerAction().Player.Attack.canceled += OnAttack;
        }
    }

    private void Update()
    {
        
        if (_isHoldingAttack )
        {
            _renderer = BezierCurve.DrawQuadraticBezierCurve(_renderer,transform.position,transform.position + new Vector3(0.5f,2,0),_cursor.transform.position);

            if (Math.Abs(_cursor.CurrentRad - _cursor.MaxRadius) > 0.01f)
            {
                _currentHoldTime += Time.deltaTime;
                UpdateHoldAttackCursor();
            }
                
        }
    }

    public void OnRotate(InputValue value)
    {
        _rotation = value.Get<Vector2>();

        if (_rotation == Vector2.zero) // idk why rider being baby but seems to work fine
        {
            _isJoystickNeutral = true;
            return;
        }

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
    
    public void HoldAttack(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            _isHoldingAttack = true;
            return;
        }

        

        var position = _cursor.transform.position;
        var instantiatedProjectile = Instantiate(_controller.GetCurrentItem().ProjectileInstance,transform.position,Quaternion.identity);

        if (_controller.GetCurrentItem().Type == ItemType.Throwable)
        {

            List<Vector2> positions = new List<Vector2>();
                
            for (int i = 0; i < _renderer.positionCount; i++)
                positions.Add((Vector2)_renderer.GetPosition(i));
                
            instantiatedProjectile.GetComponent<Throwable>().Initialise(positions.ToArray(),_controller.GetCurrentItem().ItemSprite,_controller.GetCurrentItem().DrawSpeed,_controller.GetCurrentItem().ItemDamage,_controller.GetCurrentItem().SticksOnWall);

        }
        else
            instantiatedProjectile.GetComponent<Projectile>().Initialise(position,_controller.GetCurrentItem().DrawSpeed,_controller.GetCurrentItem().ItemDamage,_controller.GetCurrentItem().SticksOnWall);

            
        _cursor.LerpToReset();
        _renderer.positionCount = 0;
        _currentHoldTime = 0f;
        _isHoldingAttack = false;
    }
    
      public void StartAttack(InputAction.CallbackContext value)
        {

            if (_controller.GetCurrentItem() == null)
                return;
            
            //NEEDS TO BE A CHECK IF USING PROJECTILES TO ALLOW FOR ON HOLD ACTIONS AND IGNORE THIS
            #region Ranged Attack
            
            
            if (_controller.GetCurrentItem().Type is ItemType.Projectile or ItemType.Throwable)
            {
                HoldAttack(value);
                return;
            }
            
            
            #endregion

            #region MeleeAttack

            if (value.performed)
                return;
            
             Vector3 playerInput;
             float angle;
             
             var position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            // var mouse = UnityEngine.Input.mousePosition;
            
            
            
             if (_playerReference.GetPlayerInput().currentControlScheme == "Gamepad")
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
            
             indicator.GetComponent<DamageIndicator>().Initialise(5,_attackFlag,_playerReference);
             _attackFlag = !_attackFlag;

            #endregion
            
        }
    
      public void UpdateItem()
      {
          
          if (_controller.GetCurrentItem() == null)
              return;
          
          
          _projectileReference = _controller.GetCurrentItem().ProjectileInstance;
          _handHelder.sprite = _controller.GetCurrentItem().ItemSprite;
          _cursor.MaxRadius = _controller.GetCurrentItem().DrawDistance;
      }

    
    
}
