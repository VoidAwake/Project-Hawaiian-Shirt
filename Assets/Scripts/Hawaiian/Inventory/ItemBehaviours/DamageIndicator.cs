using Hawaiian.Unit;
using Hawaiian.Utilities;
using UnityEngine;

namespace Hawaiian.Inventory.ItemBehaviours
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

        public IUnit User => _user;

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
            if (col.gameObject.GetComponent<Unit.Unit>() is IUnit && !_flag)
            {
                //Yucky 
                IUnit target = (IUnit) col.gameObject.GetComponent<Unit.Unit>();

                if (target == _user)
                    return;
                
                _flag = true;
            }
            
            if (col.gameObject.GetComponent<DamageIndicator>() && col.gameObject != this.gameObject)
               _user.ApplyKnockback(-_knockbackDirection, _knockbackDistance);
            
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (!col.gameObject.GetComponent<Unit.Unit>())
                OnAnimationEnd();
        }
    }
}
