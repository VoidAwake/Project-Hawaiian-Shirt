using System;
using Hawaiian.Utilities;
using UnityEditor;
using UnityEngine;

namespace Hawaiian.Unit
{
    public class DamageIndicator : MonoBehaviour
    {

        private static readonly int _alternateSlash = Animator.StringToHash("AlternateSlash");
        private static readonly int _slash = Animator.StringToHash("Slash");

        
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;
        
        [SerializeField]private GameEvent OnTakeDamage;
    
        public int Damage => _damage;


        private Animator _animator;

        private bool _flag;
        private IUnit _user;
        


        private void OnEnable()
        {
            

         //   _animator = gameObject.GetComponent<Animator>();
        }

        void Update()
        {

            transform.localPosition = new Vector3(0, 0, 0);

            //transform.Translate(Vector2.up * _speed * Time.deltaTime);
        }

        public void Initialise(int damage, bool flag, IUnit user) 
        {
            _damage = damage;
            _animator = GetComponent<Animator>();
            transform.localPosition = new Vector3(0, 0, 0);
            _animator.SetTrigger(flag ? _alternateSlash : _slash);
            _user = user;
        }
    
        public void OnAnimationEnd() => Destroy(this.gameObject);


        public void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.GetComponent<Unit>() is IUnit && !_flag)
            {
                //Yucky 
                IUnit target = (IUnit) col.gameObject.GetComponent<Unit>();

                if (target == _user)
                    return;
                
                
                _flag = true;
                col.gameObject.GetComponent<IUnit>().TakeDamage(_damage);
               // OnTakeDamage.Raise();
            }
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (!col.gameObject.GetComponent<Unit>())
                OnAnimationEnd();
        }
    }
}
