using System;
using Hawaiian.Utilities;
using UnityEngine;

namespace Hawaiian.Unit
{
    [CreateAssetMenu(order = 0, menuName = "Assets/Hawaiian/Events/IUnitIntEvent")]
    public class IUnitIntGameEvent : BaseGameEvent<Tuple<IUnit, int>>
    {
    
    }
}
