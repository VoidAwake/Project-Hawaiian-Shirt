using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Unit
{
    public class UnitPlayer : Unit
    {
        public void OnMove(InputValue value) { move = value.Get<Vector2>(); Debug.Log("Stick: " + value.Get<Vector2>()); }
        public void OnActionA(InputValue value) { isRunning = value.Get<float>() > 0.1f; Debug.Log("Button: " + value.Get<float>()); }
    }
}
