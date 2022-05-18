using System;
using Hawaiian.Utilities;
using UnityEditor;
using UnityEngine;

namespace Hawaiian.Unit
{
    public class DamageIndicator : ItemBehaviour
    {

        private static readonly int _alternateSlash = Animator.StringToHash("AlternateSlash");
        private static readonly int _slash = Animator.StringToHash("Slash");

        [SerializeField] private GameEvent OnTakeDamage;
        [SerializeField] private Vector2 _knockbackDirection;

        private Animator _animator;
        private bool _flag;
        private IUnit _user;

        public int KnockbackDistance => _knockbackDistance;
        
        void Update()
        {
            transform.localPosition = Vector3.zero;
        }

        public override void Initialise(float speed, int knockbackDistance,  bool flag, IUnit user, Vector2 knockbackDirection) 
        {
            BaseInitialise(user,speed,knockbackDistance);
            _animator = GetComponent<Animator>();
            _animator.SetTrigger(flag ? _alternateSlash : _slash);
            _user = user;
            _knockbackDirection = knockbackDirection;
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
            }
            
            if (col.gameObject.GetComponent<DamageIndicator>() && col.gameObject != this.gameObject)
               _user.ApplyKnockback(-_knockbackDirection, _knockbackDistance);
            
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (!col.gameObject.GetComponent<Unit>())
                OnAnimationEnd();
        }
    }
}
