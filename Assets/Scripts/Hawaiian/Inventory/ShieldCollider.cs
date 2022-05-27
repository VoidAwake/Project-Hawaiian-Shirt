using System;
using System.Collections;
using System.Collections.Generic;
using Hawaiian.Unit;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ShieldCollider : MonoBehaviour
{

    [SerializeField] private IUnitGameEvent _parryOccured;

    private void OnTriggerEnter2D(Collider2D col)
    {
        //TODO: Update itembehaviour to have a reference to their user
        if (col.gameObject.GetComponent<Projectile>() != null)
            _parryOccured.Raise(col.gameObject.GetComponent<Projectile>().User);
        else if (col.gameObject.GetComponent<DamageIndicator>() != null)
            _parryOccured.Raise(col.gameObject.GetComponent<DamageIndicator>().User);
    }
}
