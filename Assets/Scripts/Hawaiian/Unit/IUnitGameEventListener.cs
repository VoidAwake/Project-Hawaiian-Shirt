using Hawaiian.Unit;
using Hawaiian.Utilities;
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
