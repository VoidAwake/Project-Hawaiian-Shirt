using System;
using System.Collections;
using System.Collections.Generic;
using Hawaiian.Inventory;
using Hawaiian.PositionalEvents;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeldDefuser : HeldItemBehaviour
{
    private PositionalEventCaller _caller;
    private PlayerTreasure _lastInteractedTreasure;

    private bool canDefuse;

    private void OnEnable()
    {
        _caller = GetComponent<PositionalEventCaller>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerTreasure treasure = col.gameObject.GetComponent<PlayerTreasure>();

        if (treasure == null)
            return;

        _lastInteractedTreasure = treasure;
        UnitPlayer owner = treasure.Owner;

        if (owner == null)
            return;

        canDefuse = itemHolder.unitPlayer == owner;
    }

    protected override void UseItemActionPerformed(InputAction.CallbackContext value)
    {
        if (!canDefuse)
            return;

        base.UseItemActionPerformed(value);
    }

    protected override void UseItemActionCancelled(InputAction.CallbackContext value)
    {
        
        base.UseItemActionCancelled(value);
    }
}