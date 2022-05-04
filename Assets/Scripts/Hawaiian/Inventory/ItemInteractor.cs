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
    [SerializeField] private UnitAnimator _animator;

    //Components
    private InventoryController _controller;
    private List<LineRenderer> _lineRenderers;
    private Vector3[] _multiShotTargets;
    private Vector2 _rotation;
    private Vector3 _lastAttackPosition;
    private bool _attackFlag;
    private bool isLookingLeft;
    private bool _isJoystickNeutral = true;
    private bool _isHoldingAttack = false;
    private float _currentHoldTime;
    private float _offset = 1.1f;

    private GameObject _projectileInstance;
    private GameObject _projectileReference; //TODO: Get from item

    public bool IsAttacking => _isHoldingAttack;
    
    public Vector2 Rotation
    {
        get => _rotation;
        set => _rotation = value;
    }

    #region Monobehaviour

    

    private void Awake()
    {
        _controller = GetComponent<InventoryController>();
        _renderer = GetComponent<LineRenderer>();
        _playerReference.GetPlayerInput().actions["Attack"].performed += StartAttack;
        _playerReference.GetPlayerInput().actions["Attack"].canceled += StartAttack;
        _lineRenderers = new List<LineRenderer>();
    }

    private void FixedUpdate()
    {
        if (_isHoldingAttack )
        {
            if (_controller.GetCurrentItem().Type == ItemType.Projectile)
            {
                
                Vector3[] positions = new[]
                {
                    transform.position,
                    _cursor.transform.position 
                };
        
                _renderer.positionCount = 2;
                _renderer.SetPositions(positions);
                
                if (_controller.GetCurrentItem().IsMultiShot)
                {
                    if (_lineRenderers.Count > 0)
                    {
                        for (var i = 0; i < _lineRenderers.Count; i++)
                        {
                            LineRenderer lr = _lineRenderers[i];
                            lr.transform.localPosition = Vector3.zero;

                            var direction = (_cursor.transform.position - transform.position).normalized;

                            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                            
                            if (angle < 0)
                                angle = 360 - angle * -1;

                            angle += 20f * (i + 1);

                            var radians = angle * Mathf.Deg2Rad;

                            var x = Mathf.Cos(radians);
                            var y = Mathf.Sin(radians);
                            var targetPos = transform.position + new Vector3(x, y, 0f) * _cursor.CurrentRad;
                            
                            Vector3[] otherPositions = new[] {targetPos, _playerReference.transform.position,};

                            _multiShotTargets[i] = targetPos;
                            lr.positionCount = 2;
                            lr.SetPositions(otherPositions);
                        }
                    }
                }
            }
            else
                _renderer = BezierCurve.DrawQuadraticBezierCurve(_renderer,transform.position,transform.position + new Vector3(0.5f,2,0),_cursor.transform.position);

            if (Math.Abs(_cursor.CurrentRad - _cursor.MaxRadius) > 0.01f)
            {
                _currentHoldTime += Time.deltaTime;
                UpdateHoldAttackCursor();
            }
        }
    }
    
    #endregion

    #region ItemInteraction

    //Rotation Handling for the cursor
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
    
    //Handles when the player holds the attack for throwables and projectiles
    public void HoldAttack(InputAction.CallbackContext value)
    {
        if (_projectileInstance != null && _controller.GetCurrentItem().ReturnsToPlayer) //Guard statement for returnable projectiles to not allow for them to attack while the projectile is still active
            return; 
        
        if (value.performed)
        {
            _isHoldingAttack = true;

            if (_controller.GetCurrentItem().IsMultiShot)
            {
                
                _multiShotTargets = new Vector3[_controller.GetCurrentItem().ProjectileAmount - 1]; // -1 since the first one is not counted
                _lineRenderers = new List<LineRenderer>();
                
                for (int i = 0; i < _controller.GetCurrentItem().ProjectileAmount - 1; i++)
                {
                    GameObject instance = new GameObject();
                        
                    instance.transform.parent = transform.parent;
                    LineRenderer renderer = instance.AddComponent(typeof(LineRenderer)) as LineRenderer;
                    renderer.sortingOrder = 100;
                    _lineRenderers.Add(renderer);
                    
                    renderer.material = Resources.Load<Material>("Sprites/lineRendererMaterial");

                    renderer.startWidth = 0.2f;
                    renderer.endWidth = 0.2f;

                    Gradient gradient = new Gradient();
                    gradient.SetKeys(
                        new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) },
                        new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f) }
                    );
                    
                    renderer.colorGradient = gradient;
                }
            }
            
            return;
        }
     
        //Logic for when the player stops holding their attack
        var position = _cursor.transform.position;
        var projectiles = new List<GameObject>();
        
        if (_controller.GetCurrentItem().Type is ItemType.Projectile && _controller.GetCurrentItem().IsMultiShot)
        {
            for (int i = 0; i < _controller.GetCurrentItem().ProjectileAmount - 1; i++)
                projectiles.Add( Instantiate(_controller.GetCurrentItem().ProjectileInstance,transform.position,Quaternion.identity));
        }
        
        _projectileInstance  = Instantiate(_controller.GetCurrentItem().ProjectileInstance,transform.position,Quaternion.identity);

        if (_controller.GetCurrentItem().Type == ItemType.Throwable)
        {
            List<Vector2> positions = new List<Vector2>();
                
            for (int i = 0; i < _renderer.positionCount; i++)
                positions.Add((Vector2)_renderer.GetPosition(i));
                
            _projectileInstance.GetComponent<Throwable>().Initialise(positions.ToArray(),_controller.GetCurrentItem().ItemSprite,_controller.GetCurrentItem().DrawSpeed,_controller.GetCurrentItem().ItemDamage,_controller.GetCurrentItem().SticksOnWall);
        }
        else
        {
            _projectileInstance.GetComponent<Projectile>().Initialise(_playerReference,position,_controller.GetCurrentItem().DrawSpeed,_controller.GetCurrentItem().ItemDamage,_controller.GetCurrentItem().SticksOnWall,_controller.GetCurrentItem().ReturnsToPlayer);
            
            if (_controller.GetCurrentItem().Type is ItemType.Projectile && _controller.GetCurrentItem().IsMultiShot)
            {
                for (var i = 0; i < projectiles.Count; i++)
                {
                    GameObject projectile = projectiles[i];
                    projectile.GetComponent<Projectile>().Initialise(_playerReference, _multiShotTargets[i], _controller.GetCurrentItem().DrawSpeed, _controller.GetCurrentItem().ItemDamage, _controller.GetCurrentItem().SticksOnWall, _controller.GetCurrentItem().ReturnsToPlayer);
                }
            }
        }

        if (_lineRenderers.Count > 0)
        {
            for (int i = _lineRenderers.Count - 1; i >= 0; i--)
            {
                LineRenderer lr = _lineRenderers[i];
                _lineRenderers.Remove(_lineRenderers[i]);
                Destroy(lr.gameObject);
            }
        }
        
        _cursor.LerpToReset();
        _renderer.positionCount = 0;
        _currentHoldTime = 0f;
        _isHoldingAttack = false;
    }
    
     //*MAIN ITEM INTERACTION FUNCTION* Handles the initial processing of item interactions
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
      
      #endregion
      
      void UpdateHoldAttackCursor()
      {
          _cursor.CurrentRad += _currentHoldTime / 2;
      }

      void CancelRotation()
      {
          _rotation = Vector2.zero;
          _isJoystickNeutral = true;
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
