using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : UnitPhysics
{
    // Movement variables
    [Header("Movement Speed")]
    [SerializeField] float maxSpeed;
    [SerializeField] float tweenRate;
    [SerializeField] float runMultiplier;

    Vector2 move = new Vector2(); // for directional input
    bool controlsEnabled = true;
    bool isRunning = false;

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    protected override void SetTargetVelocity()
    {
        // Update inputs and velocity
        if (controlsEnabled) velocity = Vector2.Lerp(velocity, maxSpeed * (isRunning ? runMultiplier * move.normalized : move.normalized), Mathf.Clamp(Time.deltaTime * tweenRate, 0.0f, 1.0f));
        else velocity = Vector2.Lerp(velocity, Vector2.zero, Mathf.Clamp(Time.deltaTime * tweenRate, 0.0f, 1.0f));
    }
}
