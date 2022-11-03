using Hawaiian.Unit;
using UnityEngine;
using System.Collections;

namespace Hawaiian.Inventory.ItemBehaviours
{
    public class DamageIndicator : ItemBehaviour
    {

        private static readonly int _alternateSlash = Animator.StringToHash("AlternateSlash");
        private static readonly int _slash = Animator.StringToHash("Slash");

        [SerializeField] private Vector2 _knockbackDirection;

        private Animator _animator;
        private bool _flag;
        private IUnit _user;
        private bool _isDead;

        public IUnit User => _user;

        public int KnockbackDistance => _knockbackDistance;
        
        public void Initialise(float speed, int knockbackDistance,  bool flag, IUnit user, Vector2 knockbackDirection) 
        {
            BaseInitialise(user,speed,knockbackDistance);
            _animator = GetComponent<Animator>();
            _animator.SetTrigger(flag ? _alternateSlash : _slash);
            _user = user;
            _knockbackDirection = knockbackDirection;
        }

        public void OnAnimationEnd()
        {
            if (_isDead) return;

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (!spriteRenderer)
            {
                Destroy(gameObject);
                return;
            }

            _isDead = true;
            transform.parent = null;

            Animator animator = GetComponent<Animator>();
            if (animator)
                Destroy(animator);

            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            if (rigidbody)
                Destroy(rigidbody);

            Collider2D collider = GetComponent<Collider2D>();
            if (collider)
                Destroy(collider);

            StartCoroutine(FadeOutAndThenDestroySelf(spriteRenderer));
        }

        IEnumerator FadeOutAndThenDestroySelf(SpriteRenderer spriteRenderer, float duration = 0.15f)
        {
            float timer = 0.0f;

            while (timer < duration)
            {
                spriteRenderer.color = new Color(1, 1, 1, 1 - timer / duration);

                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            Destroy(gameObject);
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            if (_isDead) return;

            if (col.gameObject.GetComponent<Unit.Unit>() is IUnit && !_flag)
            {
                //Yucky 
                IUnit target = (IUnit) col.gameObject.GetComponent<Unit.Unit>();

                if (target == _user)
                    return;
                
                _flag = true;
            }

            // If this hitbox impacts with another, knock our user backwards
            if (col.gameObject.GetComponent<DamageIndicator>() && col.gameObject != this.gameObject) // Checking if we hit ourselves doesn't actually work here?
                _user.ApplyKnockback(-_knockbackDirection, _knockbackDistance);//, 0.2f);
            
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (_isDead) return;

            if (!col.gameObject.GetComponent<Unit.Unit>())
                OnAnimationEnd();
        }
    }
}
