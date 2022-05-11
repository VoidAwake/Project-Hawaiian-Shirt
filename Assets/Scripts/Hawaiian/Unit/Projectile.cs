using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Hawaiian.Unit
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private bool _canStickOnWalls;
        [SerializeField] private float _damage;
        [SerializeField] private bool _returnsToPlayer;
        [SerializeField] private bool _isRicochet;
        [SerializeField] private int _maximumBounces;

        private bool hasReachedDestination;

        private float _currentDistance;
        private Vector2 _targetLocation;
        private IUnit _user;
        private bool hasCollided = false;
        private int _currentBounces;

        public float Speed => _speed;
        public bool CanStickOnWalls => _canStickOnWalls;
        public float Damage => _damage;
        public Vector2 Direction { get; private set; }

        public void Initialise(Vector3 target)
        {
            _targetLocation = target;
        }

        public void Initialise(IUnit user, Vector3 target, float speed = 5, float damage = 5,
            bool canStickOnWalls = false, bool returnsToPlayer = false, bool ricochet = false, int maximumBounce = 0)
        {
            _targetLocation = target;
            _speed = speed;
            _damage = damage;
            _canStickOnWalls = canStickOnWalls;
            _returnsToPlayer = returnsToPlayer;
            _user = user;
            hasReachedDestination = false;
            _isRicochet = ricochet;
            _maximumBounces = maximumBounce;


            Direction = _targetLocation - (Vector2) transform.position;
        }

        private void Update()
        {
            if (hasCollided && _isRicochet)
            {
                if (_currentBounces >= _maximumBounces)
                    Destroy(this.gameObject);

                _currentBounces++;

                hasCollided = false;
                Vector3 inNormal;

                if (Mathf.Abs(Direction.x) > Mathf.Abs(Direction.y))
                    inNormal = Direction.x >= 0 ? Vector2.up : Vector2.down;
                else
                    inNormal = Direction.y >= 0 ? Vector2.right : Vector2.left;

                Direction = Vector3.Reflect(Direction, inNormal);
                Direction *= -1;
                _targetLocation = Direction * 1.5f;
            }

            var step = _speed * Time.deltaTime;

            if (hasReachedDestination)
            {
                transform.position = Vector3.MoveTowards(transform.position, _user.GetPosition(), step);

                if (Vector3.Distance(transform.position, _user.GetPosition()) < 0.01f)
                {
                    Destroy(this.gameObject);
                }

                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, _targetLocation, step);

            if (Vector3.Distance(transform.position, _targetLocation) < 0.01f)
            {
                if (_returnsToPlayer)
                {
                    hasReachedDestination = true;
                    return;
                }

                Destroy(this.gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.GetComponent<Unit>() || !other.gameObject.GetComponent<Projectile>())
                hasCollided = true;
            
        }
    }
}