using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitEnemy : Unit
{
    // Start is called before the first frame update
    public void OnMove(InputValue value) { move = value.Get<Vector2>(); Debug.Log("Stick: " + value.Get<Vector2>()); }
}
