using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class IUnitGameEventListener : BaseGameEventListener<IUnit>
{
   
    public void UpdateEvent(IUnitGameEvent newGameEvent, UnityEvent<IUnit> processParry)
    {
        Event = newGameEvent;
        Response = processParry;
        OnEnable();
    }
}
