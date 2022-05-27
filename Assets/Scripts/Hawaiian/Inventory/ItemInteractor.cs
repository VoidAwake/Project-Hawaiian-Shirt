using System;
using System.Collections.Generic;
using Hawaiian.Inventory;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InventoryController))]
public class ItemInteractor : MonoBehaviour
{
    [Header("Components")] [SerializeField]
    private LineRenderer _renderer;

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
    private float _slashCooldown;

   [SerializeField] private IUnitGameEvent _removeItem;

    private GameObject _projectileInstance;
    private GameObject _projectileReference; //TODO: Get from item

    public bool IsAttacking => _isHoldingAttack;


    public Item CurrentItem => _controller.CurrentItem;

    public Vector2 Rotation
    {
        get => _rotation;
        set => _rotation = value;
    }

    private Vector2 _move;

    public bool CanMeleeAttack() => _slashCooldown <= 0;

    public bool signal = false;

    #region Monobehaviour

    private void Awake()
    {
        _controller = GetComponent<InventoryController>();
        _renderer = GetComponent<LineRenderer>();
        _playerReference.GetPlayerInput().actions["Attack"].performed += StartAttack;
        _playerReference.GetPlayerInput().actions["Attack"].canceled += StartAttack;
        _lineRenderers = new List<LineRenderer>();
        // TODO: Unlisten?
        _controller.currentItemChanged.AddListener(OnCurrentItemChanged);
    }

    private void FixedUpdate()
    {
        if (_isHoldingAttack)
        {
            if (_controller.CurrentItem.Type == ItemType.Projectile)
                UpdateLineRenderers();
            else
                _renderer = BezierCurve.DrawQuadraticBezierCurve(_renderer, transform.position,
                    transform.position + new Vector3(0.5f, 2, 0), _cursor.transform.position);

            if (Math.Abs(_cursor.CurrentRad - _cursor.MaxRadius) > 0.01f)
            {
                _currentHoldTime += Time.deltaTime;
                UpdateHoldAttackCursor();
            }
        }
    }

    public void Update()
    {
        if (_slashCooldown >= 0)
            _slashCooldown -= Time.deltaTime;
    }

    #endregion


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

    public void OnMoveCursor(InputValue value)
    {
        _rotation = value.Get<Vector2>();
    }

    public void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
    }


    //Handles when the player holds the attack for throwables and projectiles
    public void HoldAttack(InputAction.CallbackContext value)
    {
        if (_projectileInstance != null &&
            _controller.CurrentItem
                .ReturnsToPlayer) //Guard statement for returnable projectiles to not allow for them to attack while the projectile is still active
            return;

        if (_projectileInstance != null && _controller.CurrentItem.Type == ItemType.Throwable)
            return;

        if (value.performed)
        {
            _isHoldingAttack = true;

            _multiShotTargets =
                new Vector3[CurrentItem.ProjectileAmount == 0 ? 1 : CurrentItem.ProjectileAmount];
            _lineRenderers = new List<LineRenderer>();

            GenerateLineRenderers();
            return;
        }

        var projectiles = new List<GameObject>();

        for (int i = _controller.CurrentItem.ProjectileAmount == 0 ? -1 : 0;
             i < _controller.CurrentItem.ProjectileAmount;
             i++)
        {

             _projectileInstance = Instantiate(_controller.CurrentItem.ProjectileInstance, transform.position, Quaternion.identity);
            projectiles.Add(_projectileInstance);

            if (i == -1)
                break;
        }

        switch (CurrentItem.Type)
        {
            case ItemType.Throwable:
                UseItem<Throwable>(projectiles);
                break;
            case ItemType.Projectile:
                UseItem<Projectile>(projectiles);
                break;
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
        if (_controller.CurrentItem == null) return;

        if (_controller.CurrentItem.Type is ItemType.Other or ItemType.Objective) return;


        #region Ranged Attack

        if (_controller.CurrentItem.Type is ItemType.Projectile or ItemType.Throwable)
        {
            HoldAttack(value);
            return;
        }

        if (_controller.CurrentItem.Type == ItemType.Trap)
        {
            BeginTrapHighlighting();
            return;
        }

        #endregion

        #region MeleeAttack

        if (value.performed) return;

        if (!CanMeleeAttack()) return;

        UseItem<DamageIndicator>();
    }

    #endregion

    private void BeginTrapHighlighting()
    {
        _cursor.CurrentRad = _controller.CurrentItem.PlacementRadius;

        GameObject instanceCircle = new GameObject();
        SpriteRenderer renderer = instanceCircle.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        renderer.sprite = Resources.Load<Sprite>("Circle");
        var o = renderer.gameObject;
        o.transform.parent = _playerReference.transform;
        o.transform.localPosition = Vector3.zero;
        renderer.gameObject.transform.localScale = new Vector3(_controller.CurrentItem.PlacementRadius,
            _controller.CurrentItem.PlacementRadius, 0);
        renderer.color = new Color32(255, 109, 114, 170);
        renderer.sortingOrder = 1;
    }

    void UpdateHoldAttackCursor() => _cursor.CurrentRad += _currentHoldTime / 0.5f;


    void CancelRotation()
    {
        _rotation = Vector2.zero;
        _isJoystickNeutral = true;
    }

    public void OnCurrentItemChanged()
    {
        if (_controller.CurrentItem == null) return;


        _projectileReference = _controller.CurrentItem.ProjectileInstance;
        _handHelder.sprite = _controller.CurrentItem.ItemSprite;
        _cursor.MaxRadius = _controller.CurrentItem.DrawDistance;
    }

    private void UseItem<T>(List<GameObject> projectiles = null) where T : ItemBehaviour
    {
        List<Vector2> positions = new List<Vector2>();

        for (int i = 0; i < _renderer.positionCount; i++)
            positions.Add((Vector2) _renderer.GetPosition(i));

        int index = 0;

        if (projectiles != null)
        {
            projectiles.ForEach(p =>
            {
                p.GetComponent<T>()
                    .BaseInitialise(_playerReference, CurrentItem.DrawSpeed, CurrentItem.KnockbackDistance);

                switch (p.GetComponent<T>())
                {
                    case Throwable:
                        p.GetComponent<T>().Initialise(positions.ToArray(), CurrentItem.ItemSprite,
                            CurrentItem.SticksOnWall);  
                        _removeItem.Raise(_playerReference);
                        break;
                    case Projectile:
                        p.GetComponent<T>().Initialise(_playerReference, _multiShotTargets[index],
                            CurrentItem.SticksOnWall, CurrentItem.ReturnsToPlayer, CurrentItem.IsRicochet,
                            CurrentItem.MaximumBounces);
                        break;
                }

                if (p.GetComponent<HitUnit>())
                    p.GetComponent<HitUnit>()
                        .Initialise(_playerReference, _cursor.transform.position - transform.position);
                
                

                transform.parent.GetComponent<UnitAnimator>()
                    .UseItem(UnitAnimationState.Throw, _cursor.transform.position, false);

                index++;
            });
        }
        else
        {
            //Begin melee 
            Vector3 input = GetPlayerInput();
            var angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            var direction = input;
            InstantiateMeleeIndicator(angle, direction);
        }

    }

    private void InstantiateMeleeIndicator(float angle, Vector3 direction)
    {
        _firePoint.position = _lastAttackPosition;

        transform.parent.GetComponent<UnitAnimator>()
            .UseItem(UnitAnimationState.MeleeSwing,
                new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad + Mathf.PI / 2),
                    -Mathf.Cos(angle * Mathf.Deg2Rad + Mathf.PI / 2)), _attackFlag);

        GameObject indicator = Instantiate(_projectileReference, _lastAttackPosition,
            Quaternion.Euler(new Vector3(0, 0, angle + _meleeSlashRotationOffset)), _firePoint);

        indicator.GetComponent<DamageIndicator>().Initialise(CurrentItem.DrawSpeed,CurrentItem.KnockbackDistance,_attackFlag, _playerReference, direction);
        indicator.GetComponent<HitUnit>().Initialise(_playerReference, _cursor.transform.position - transform.position);
        _attackFlag = !_attackFlag;
    }

    private Vector3 GetPlayerInput()
    {
        _slashCooldown = _controller.CurrentItem.AttackRate;
        Vector3 playerInput;

        var position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);


        //if (_playerReference.GetPlayerInput().currentControlScheme == "Gamepad")
        //{
        //    if (_isJoystickNeutral) _rotation = _animator.IsLookingLeft ? Vector2.left : Vector2.right;
        //}

        //playerInput = _rotation;

        Vector3 prevInput = (_cursor.transform.localPosition - Vector3.up * 0.5f);
        playerInput = prevInput.magnitude == 0 ? Vector2.right.normalized : prevInput.normalized;

        _lastAttackPosition = position + (Vector3) playerInput * _offset;
        return playerInput;
    }


    private void GenerateLineRenderers()
    {
        for (int i = 0; i < (CurrentItem.ProjectileAmount == 0 ? 1 : CurrentItem.ProjectileAmount); i++)
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
                new GradientColorKey[]
                {
                    new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f)
                }, new GradientAlphaKey[] {new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f)});

            renderer.colorGradient = gradient;
        }
    }

    private void UpdateLineRenderers()
    {
        if (_lineRenderers.Count > 0)
        {
            for (var i = 0; i < _lineRenderers.Count; i++)
            {
                LineRenderer lr = _lineRenderers[i];
                lr.transform.localPosition = Vector3.zero;

                var direction = (_cursor.transform.position - transform.position).normalized;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                if (angle < 0) angle = 360 - angle * -1;

                angle += _lineRenderers.Count == 1 ? 0 : (i == 0 ? 0 : 20f) * (i + 1);

                var radians = angle * Mathf.Deg2Rad;

                var x = Mathf.Cos(radians);
                var y = Mathf.Sin(radians);
                var targetPos = transform.position + new Vector3(x, y, 0f) * _cursor.CurrentRad;

                Vector3[] otherPositions = new[] {targetPos, _playerReference.transform.position + Vector3.up * 0.5f,};

                _multiShotTargets[i] = targetPos;
                lr.positionCount = 2;
                lr.SetPositions(otherPositions);
            }
        }
    }

    public void OnDrop()
    {
        //_controller.DropItLikeItsHot(_rotation);
        Vector3 prevInput = (_cursor.transform.localPosition - Vector3.up * 0.5f);
        Vector3 playerInput = prevInput.magnitude == 0 ? Vector2.right.normalized : prevInput.normalized;

        _controller.DropItLikeItsHot(playerInput);
    }
}