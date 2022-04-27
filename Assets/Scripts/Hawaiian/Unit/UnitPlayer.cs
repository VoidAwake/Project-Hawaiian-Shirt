using System;
using Hawaiian.Input;
using Hawaiian.Utilities;
using MoreLinq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Unit
{
    public class UnitPlayer : Unit, IUnit
    {
        [SerializeField] private GameEvent forward;
        [SerializeField] private GameEvent backward;
        [SerializeField] private GameObject _damageReference;
        [SerializeField] private GameObject firePoint;

        [SerializeField] private PlayerAction _input;


        private bool _attackFlag = false;
        protected override void Awake()
        {
            base.Awake();
            _input = new PlayerAction();
        }

        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        protected override void Start()
        {
            base.Start();
            //forward.Raise();
            _input.Player.Parse.performed += context =>
            {
                if (context.ReadValue<float>() > 0f)
                {
                    forward.Raise();
                }
                else
                {
                    backward.Raise();
                }
            };

        }

        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        } // Debug.Log("Stick: " + value.Get<Vector2>()); }
        public void OnActionA(InputValue value) { isRunning = value.Get<float>() > 0.1f;} //Debug.Log("Button: " + value.Get<float>()); }


        public void OnAttack(InputValue value)
        {
         //   GameObject indicator = Instantiate(_damageReference, firePoint.transform.position, Quaternion.Euler(0,0,-90));
            GameObject indicator = Instantiate(_damageReference, firePoint.transform);

            indicator.GetComponent<DamageIndicator>().Initialise(5,_attackFlag,this);
            _attackFlag = !_attackFlag;
        }
    

        public void TakeDamage(float damage)
        {
            _health -= damage;
            OnDeath();
        }

        public void OnDeath()
        {
            if (_health > 0)
                return;
            
            Destroy(this.gameObject);
        }

        public void Use() { }
    }
}
