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
        [SerializeField] private GameEvent OnTakeDamage;
        
        private Animator _animator;
        private bool _flag;
        private IUnit _user;
        
        public int Damage => _damage;
        
        void Update()
        {
            transform.localPosition = Vector3.zero;
        }

        public void Initialise(int damage, bool flag, IUnit user) 
        {
            _damage = damage;
            _animator = GetComponent<Animator>();
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
                // col.gameObject.GetComponent<IUnit>().TakeDamage(_damage);
            }
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (!col.gameObject.GetComponent<Unit>())
                OnAnimationEnd();
        }
    }
}
