using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Codice.Client.Common.GameUI;
using Hawaiian.Inventory;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using TMPro;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.Events;

public class Shield : ItemBehaviour
{
    [SerializeField] [Tooltip("0 is down, 1 is up")]
    private Sprite[] _shieldState;

    private GameObject _shieldCollider;
    private GameObject _shieldColliderInstance;

    private SpriteRenderer _shieldSprite;
    private float _duration;
    private bool shieldLifted;

    public float ParryWindow { get; set; }
    public float TimeTillParry { get; set; }

    public int[] ParryPercentage { get; set; }

    private GameEvent parryOccured;

    private IUnitGameEventListener _listener;

    private ItemInteractor _itemInteractor;

    private bool _canParry;
    private UnitPlayer _user;

    public void Initialise(float parryWindow, int[] parryPercentage, SpriteRenderer spriteReference,
        Sprite[] shieldStates, IUnitGameEvent parryoccured, GameObject shieldCollider, float timeTillParry, UnitPlayer user)
    {
        _user = user;
        TimeTillParry = timeTillParry;
        _shieldCollider = shieldCollider;
        IUnitGameEventListener _eventListener = gameObject.AddComponent<IUnitGameEventListener>();
        _eventListener.UpdateEvent(parryoccured, new UnityEvent<IUnit>());
        _eventListener.Response.AddListener(ProcessParry);
        _shieldSprite = spriteReference;
        _shieldState = shieldStates;
        ParryWindow = parryWindow;
        ParryPercentage = parryPercentage;
        _duration = 0;
        _itemInteractor = GetComponent<ItemInteractor>();
        _canParry = true;
    }

    public void OnDisable()
    {
        if (_listener != null)
            _listener.Response.RemoveListener(ProcessParry);
    }

    public bool CanParry() => _canParry;

    public void ProcessParry(IUnit victim)
    {

        if (GetComponentInParent<UnitPlayer>().playerState == Unit.PlayerState.Tripped)
            return;
        
        _itemInteractor.PlayerReference.OverrideDamage = true;

        if ((_duration / ParryWindow) * 100 <= ParryPercentage[1] &&
            (_duration / ParryWindow) * 100 >= ParryPercentage[0])
        {
            //Perfect Parry
            Debug.Log("PERFECT PARRY");

            if (_shieldColliderInstance == null)
            {
                StartCoroutine(RemoveOverrideDamage());
                return;
            }

            if (_shieldCollider.GetComponent<ShieldCollider>() != null)
            {
                ShieldCollider collider = _shieldColliderInstance.GetComponent<ShieldCollider>();
                if (collider.ProjectileInstance != null)
                {
                    collider.ProjectileInstance.UpdateTargetToFinalDestination(
                        victim.GetPosition() - transform.position, 2f);
                }
                else
                {
                    victim.TripUnit(victim.GetPosition() - transform.position, 8); //TODO Change distance
                    var inventoryController = victim.GetUnit().GetComponentInChildren<InventoryController>();

                    if (inventoryController != null)
                        inventoryController.DropRandom(-(victim.GetPosition() - transform.position));
                    
                }
            }
        }
        else
        {
            
            Debug.Log("PERFECT PARRY");

            if (_shieldCollider.GetComponent<ShieldCollider>() != null)
            {
                ShieldCollider collider = _shieldColliderInstance.GetComponent<ShieldCollider>();
                if (collider.ProjectileInstance != null)
                {
                    collider.ProjectileInstance.UpdateTargetToFinalDestination(
                        victim.GetPosition() - transform.position, 2f);
                }
                else
                {
                    victim.TripUnit(victim.GetPosition() - transform.position, 2); //TODO Change distance
                }
            }
        }

        StartCoroutine(RemoveOverrideDamage());
    }

    
    //Makes sure that if a parry is successful synchronous to the player being hit, that the hit will not occur
    IEnumerator RemoveOverrideDamage()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        _itemInteractor.PlayerReference.OverrideDamage = false;


    }


    // If the unit was not hit during the parry, ensures that the override tag is removed


    public void LiftShield()
    {
        Debug.LogWarning("Currently Parrying");
        shieldLifted = true;
        _canParry = false;
        _shieldColliderInstance = Instantiate(_shieldCollider, transform);
        _shieldSprite.sprite = _shieldState[1];
    }

    public void UnliftShield()
    {
        if (_shieldColliderInstance != null)
            Destroy(_shieldColliderInstance);
        
        StartCoroutine(ShieldCooldown(TimeTillParry));
        shieldLifted = false;
        _shieldSprite.sprite = _shieldState[0];
        _duration = 0;

     
    }

    public void RemoveShieldComponent()
    {
        if (_shieldColliderInstance != null)
            Destroy(_shieldColliderInstance);

        Destroy(this);
    }

    IEnumerator ShieldCooldown(float timer)
    {
        _canParry = false;
        yield return new WaitForSeconds(timer);
        _canParry = true;
    }


    private void Update()
    {
        if (shieldLifted)
        {
            _duration += Time.deltaTime;

            if (_duration > ParryWindow)
            {
                UnliftShield();
            }
        }
    }
}