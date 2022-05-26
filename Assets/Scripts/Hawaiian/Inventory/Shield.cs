using System;
using System.Collections;
using System.Collections.Generic;
using Hawaiian.Unit;
using UnityEngine;

public class Shield : ItemBehaviour
{

    [SerializeField] [Tooltip("0 is down, 1 is up")]
    private Sprite[] _shieldState;
    
    
    public float ParryWindow { get; set; }

    public int ParryPercentage { get; set; }

    public override void Initialise(float parryWindow, int parryPercentage, SpriteRenderer spriteReference)
    {
        base.Initialise(parryWindow, parryPercentage,spriteReference);
    }

    public void LiftShield()
    {
        
    }
    
    
}
