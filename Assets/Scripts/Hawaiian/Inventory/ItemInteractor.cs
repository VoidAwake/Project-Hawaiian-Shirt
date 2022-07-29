using System;
using System.Collections.Generic;
using Hawaiian.Inventory;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.InputSystem;
using Cursor = Hawaiian.Inventory.Cursor;

[RequireComponent(typeof(InventoryController))]
public class ItemInteractor : MonoBehaviour
{
    public static readonly int Rate = Shader.PropertyToID("_Rate");
    private static readonly int LineColour = Shader.PropertyToID("_lineColour");


    [Header("Components")]

    [SerializeField] private Cursor _cursor;
    [SerializeField] public UnitPlayer _playerReference;
    [SerializeField] private SpriteRenderer _handHelder;
    [SerializeField] private IUnitGameEvent _parryOccured;
    [SerializeField] private GameObject shieldColliderPrefab;
    
    //Components
    private InventoryController _controller;
    private Vector2 _rotation;
    private bool isLookingLeft;
    private bool _isJoystickNeutral = true;
    public bool _isHoldingAttack = false;
    private float _currentHoldTime;
    private float _parryTimer;

    private Shield _shieldReference;

    public bool IsAttacking => _isHoldingAttack;

    public UnitPlayer PlayerReference => _playerReference;

    public Item CurrentItem => _controller.CurrentItem;

    #region Monobehaviour

    private void Awake()
    {
        _controller = GetComponent<InventoryController>();
        _playerReference.GetPlayerInput().actions["Attack"].performed += StartAttack;
        _playerReference.GetPlayerInput().actions["Attack"].canceled += StartAttack;
        // TODO: Unlisten?
        _controller.currentItemChanged.AddListener(OnCurrentItemChanged);
       
    }

    private void OnDestroy()
    {
        _playerReference.GetPlayerInput().actions["Attack"].performed -= StartAttack;
        _playerReference.GetPlayerInput().actions["Attack"].canceled -= StartAttack;
    }

    private void FixedUpdate()
    {
        if (_isHoldingAttack)
        {
            if (Math.Abs(_cursor.CurrentRad - _cursor.MaxRadius) > 0.01f)
            {
                _currentHoldTime += Time.deltaTime;
                UpdateHoldAttackCursor();
            }
        }
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

    //Handles when the player holds the attack for throwables and projectiles
    public void HoldAttack(InputAction.CallbackContext value)
    {
        if (value.canceled)
        {
            _cursor.LerpToReset();
            _currentHoldTime = 0f;
            _isHoldingAttack = false;
        }
        else
        {
            _isHoldingAttack = true;
        }
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

        if (value.canceled) return;

        if (CurrentItem.Type == ItemType.Shield)
        {
            UseItem<Shield>();
            return;
        }
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
        if (_controller.CurrentItem == null)
        {
            if (_shieldReference != null)
                _shieldReference.RemoveShieldComponent();

            return;
        }

        if (CurrentItem.Type == ItemType.Shield)
        {
            _shieldReference = gameObject.AddComponent<Shield>();
            _shieldReference.Initialise(CurrentItem.ParryWindow,
                new[] {CurrentItem.ParryPercentageUpperLimit, CurrentItem.ParryPercentageLowerLimit}, _handHelder,
                new[] {CurrentItem.ShieldDown, CurrentItem.ShieldUp}, _parryOccured, shieldColliderPrefab,
                CurrentItem.TimeTillParry, _playerReference, _cursor.transform);
        }
        else if (_shieldReference != null)
            _shieldReference.RemoveShieldComponent();

        _handHelder.sprite = _controller.CurrentItem.ItemSprite;
        _cursor.MaxRadius = _controller.CurrentItem.DrawDistance;
    }

    private void UseItem<T>(List<GameObject> projectiles = null) where T : ItemBehaviour
    {
        if (CurrentItem.Type == ItemType.Shield && _shieldReference.CanParry())
            _shieldReference.LiftShield();
    }

    // TODO: Why is this here when all the other inputs are handled by InventoryController?
    public void OnDrop()
    {
        // Prevent items being dropped while attacking
        if (GetComponent<ItemInteractor>().IsAttacking) return;
        
        //_controller.DropItLikeItsHot(_rotation);
        Vector3 prevInput = (_cursor.transform.localPosition - Vector3.up * 0.5f);
        Vector3 playerInput = prevInput.magnitude == 0 ? Vector2.right.normalized : prevInput.normalized;

        _controller.DropItLikeItsHot(playerInput);
    }
}