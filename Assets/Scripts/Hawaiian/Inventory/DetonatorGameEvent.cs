using System;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using UnityEngine;

namespace Hawaiian.Inventory
{
    [CreateAssetMenu(order = 0, menuName = "Hawaiian/Events/DetonatorGameEvent")]
    public class DetonatorGameEvent : BaseGameEvent<Tuple<IUnit,Detonator>>
    {
    
    }
}
