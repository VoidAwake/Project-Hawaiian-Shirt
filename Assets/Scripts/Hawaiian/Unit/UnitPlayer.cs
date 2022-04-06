using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Unit
{
    public class UnitPlayer : Unit, IUnit
    {
        
        [SerializeField] private GameObject _damageReference;
        [SerializeField] private GameObject firePoint;


        private bool _attackFlag = false;

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
