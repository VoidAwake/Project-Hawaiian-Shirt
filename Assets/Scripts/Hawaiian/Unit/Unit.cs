using System;
using UnityEngine;

namespace Hawaiian.Unit
{
    public class Unit : UnitPhysics
    {

        [Header("Unit Stats")]
        [SerializeField]internal float _maxHealth;

        internal float _health;

    
        public float Health => _health;
        
        // Movement variables
        [Header("Movement Speed")]
        [SerializeField] internal float maxSpeed;
        [SerializeField] internal float tweenRate;
        [SerializeField] internal float runMultiplier;

        protected Vector2 move = new Vector2(); // for directional input
        protected bool controlsEnabled = true;
        protected bool isRunning = false;

        protected void Start()
        {
            _health = _maxHealth;
        }

        protected override void SetTargetVelocity()
        {
            // Update inputs and velocity
            if (controlsEnabled) velocity = Vector2.Lerp(velocity, maxSpeed * (isRunning ? runMultiplier * move.normalized : move.normalized), Mathf.Clamp(Time.deltaTime * tweenRate, 0.0f, 1.0f));
            else velocity = Vector2.Lerp(velocity, Vector2.zero, Mathf.Clamp(Time.deltaTime * tweenRate, 0.0f, 1.0f));
        }

   

     
    }
}
