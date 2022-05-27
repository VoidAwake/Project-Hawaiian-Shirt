using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.Common.GameUI;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using TMPro;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

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
    public int ParryPercentage { get; set; }
    
    private GameEvent parryOccured;

    private IUnitGameEventListener _listener;

    private ItemInteractor _itemInteractor;

    public void Initialise(float parryWindow, int parryPercentage, SpriteRenderer spriteReference, Sprite[] shieldStates, IUnitGameEvent parryoccured, GameObject shieldCollider)
    {
        _shieldCollider = shieldCollider;
        IUnitGameEventListener _eventListener = gameObject.AddComponent<IUnitGameEventListener>();
        _eventListener.UpdateEvent(parryoccured,new UnityEvent<IUnit>());
        _eventListener.Response.AddListener(ProcessParry);
        _shieldSprite = spriteReference;
        _shieldState = shieldStates;
        ParryWindow = parryWindow;
        ParryPercentage = parryPercentage;
        _duration = 0;
        _itemInteractor = GetComponent<ItemInteractor>();
    }

    public void OnDisable()
    {
        if (_listener != null)
            _listener.Response.RemoveListener(ProcessParry);
        
    }

    public void ProcessParry(IUnit victim)
    {
        _itemInteractor.PlayerReference.OverrideDamage = true;
        
        if ((_duration / ParryWindow) * 100 <= ParryPercentage)
        {
            //Perfect Parry
            Debug.Log("PERFECT PARRY");
           victim.TripUnit(victim.GetPosition() - transform.position,8); //TODO Change distance
        }
        else
        {
            victim.TripUnit(victim.GetPosition() - transform.position,2); //TODO Change distance
            Debug.Log("PARTIAL PARRY");
        }
    
        StartCoroutine(RemoveOverride());
    }

    
    // If the unit was not hit during the parry, ensures that the override tag is removed
    IEnumerator RemoveOverride()
    {
        yield return new WaitForSeconds(0.2f);
        
        if (_itemInteractor.PlayerReference.OverrideDamage)
            _itemInteractor.PlayerReference.OverrideDamage = false;
    }

    public void LiftShield()
    {
        _shieldColliderInstance = Instantiate(_shieldCollider, transform);
        _shieldSprite.sprite = _shieldState[1];
        shieldLifted = true;
    }
    
    public void UnliftShield()
    {
        shieldLifted = false;
        _shieldSprite.sprite = _shieldState[0];
        _duration = 0;
        
        if(_shieldColliderInstance != null)
            Destroy(_shieldColliderInstance);
       
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
