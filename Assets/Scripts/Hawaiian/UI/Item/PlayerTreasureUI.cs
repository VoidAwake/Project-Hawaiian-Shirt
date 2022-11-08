using System.Collections;
using System.Collections.Generic;
using Hawaiian.Inventory;
using UI.Core;
using UnityEngine;

public class PlayerTreasureUI : Dialogue
{

    [SerializeField] internal PlayerTreasure _playerTreasure;
    
    protected override void OnClose() { }

    protected override void OnPromote()
    {
        _playerTreasure = GetComponentInParent<PlayerTreasure>();
    }

    protected override void OnDemote() { }

  
}
