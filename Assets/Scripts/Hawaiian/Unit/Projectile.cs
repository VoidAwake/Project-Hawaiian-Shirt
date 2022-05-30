using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Hawaiian.Unit
{
    public class Projectile : ItemBehaviour
    {
        [SerializeField] private bool _canStickOnWalls;
        [SerializeField] private bool _returnsToPlayer;
        [SerializeField] private bool _isRicochet;
        [SerializeField] private int _maximumBounces;
        [SerializeField] private AnimationCurve _speedCurve;
        [SerializeField] private AnimationCurve _returnToPlayerCurve;


        private bool hasReachedDestination;

        private float _currentDistance;
        private Vector2 _targetLocation;
        private IUnit _user;
        private bool hasCollided = false;
        private int _currentBounces;
        private float totalDistance;
        private float maxSpeed;
        private bool isOnWall;
        
        public float Speed => _speed;
        public bool CanStickOnWalls => _canStickOnWalls;
        public Vector2 Direction { get; private set; }

        public void Initialise(Vector3 target)
        {
            _targetLocation = target;
        }

        public override void Initialise(IUnit user, Vector3 target, bool canStickOnWalls = false,
            bool returnsToPlayer = false, bool ricochet = false, int maximumBounce = 0)
        {
            
            isOnWall = false;
            _targetLocation = target;
            maxSpeed = _speed;
            _canStickOnWalls = canStickOnWalls;
            _returnsToPlayer = returnsToPlayer;
            _user = user;
            hasReachedDestination = false;
            _isRicochet = ricochet;
            _maximumBounces = maximumBounce;
            totalDistance = Vector3.Distance(transform.position, _targetLocation);
            Direction = _targetLocation - (Vector2) transform.position;
        }

        private void Update()
        {
            if (hasCollided)
            {
                if (_canStickOnWalls)
                {
                    StartCoroutine(SelfDestruct());

                } else if (_isRicochet)
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
                else
                {
                    Destroy(gameObject);
                }
            }

            var distance = Vector3.Distance(transform.position, _targetLocation);
            var calculatedDistance = distance / totalDistance;

            _speed = !hasReachedDestination
                ? _speedCurve.Evaluate(1 - calculatedDistance) * maxSpeed
                : _returnToPlayerCurve.Evaluate(1 - calculatedDistance) * maxSpeed;

            var step = _speed * Time.deltaTime;

            if (hasReachedDestination)
            {
                transform.position = Vector3.MoveTowards(transform.position, _user.GetPosition(), step * 2);

                if (Vector3.Distance(transform.position, _user.GetPosition()) < 0.1f)
                {
                    Destroy(this.gameObject);
                }

                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, _targetLocation, step);

            if (Vector3.Distance(transform.position, _targetLocation) < 0.1f)
            {
                if (_returnsToPlayer)
                {
                    hasReachedDestination = true;
                    _targetLocation = _user.GetPosition();
                    totalDistance = Vector3.Distance(transform.position, _targetLocation);
                    return;
                }

                Destroy(this.gameObject);
            }
        }

        IEnumerator SelfDestruct()
        {
            isOnWall = true;
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.GetComponent<UnitPlayer>() && !other.gameObject.GetComponent<Projectile>() &&
                !other.gameObject.GetComponent<ItemUnit>() && !other.gameObject.GetComponent<UnitDrop>())

        {
                    Debug.Log(!other.gameObject.GetComponent<UnitPlayer>() + " state of the unit player");
                Debug.Log(!other.gameObject.GetComponent<Projectile>() + " state of the projectile");
                Debug.Log(!other.gameObject.GetComponent<ItemUnit>() + " state of the item unit");

                hasCollided = true;
            }
        }

        public bool IsOnWall() => isOnWall;
        
        
    }
}