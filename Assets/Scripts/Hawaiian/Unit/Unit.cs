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
            Vector2 modifiedMove = move.magnitude * 2.0f > 1.0f ? move.normalized : move.magnitude < 0.1f ? Vector2.zero : move * 2.0f;
            if (controlsEnabled) velocity = Vector2.Lerp(velocity, maxSpeed * (isRunning ? runMultiplier * modifiedMove : modifiedMove), Mathf.Clamp(Time.deltaTime * tweenRate, 0.0f, 1.0f));
            else velocity = Vector2.Lerp(velocity, Vector2.zero, Mathf.Clamp(Time.deltaTime * tweenRate, 0.0f, 1.0f));
        }

        public Vector2 GetVelocity()
        {
            return velocity;
        }

        public bool GetIsFullSpeed()
        {
            return isRunning && move.magnitude > 0.1f || move.magnitude >= 0.5f;
        }
     
    }
}
