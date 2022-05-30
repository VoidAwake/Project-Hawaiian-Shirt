using Hawaiian.Unit;
using UnityEngine;

public class Throwable : ItemBehaviour
{
    
    [SerializeField] internal bool _canStickOnWalls;

    private SpriteRenderer _renderer;
    private float _currentDistance;
    private Vector2[] _positions;
    private int positionIndex;
    
    public float Speed => _speed;
    public bool CanStickOnWalls => _canStickOnWalls;
    public Vector2 LastPosition => _positions[^1]; // ^1 

    private bool hasCollided;
   

    public override void Initialise(Vector2 [] targets, Sprite newSprite,bool canStickOnWalls = false)
    {
        hasCollided = false;
        _positions = targets;
        _canStickOnWalls = canStickOnWalls;
        positionIndex = 0;
        _renderer.sprite = newSprite;
    }

    private void Awake()
    {
        positionIndex = 0;
        _renderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Update()
    {
        if (hasCollided)
        {
            OnTargetReached();
            return;
        }
        
        if (_positions == null)
            return;

        if (positionIndex >= _positions.Length - 1)
            OnTargetReached();
        
        var step = _speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, _positions[positionIndex], step);
        
        if (Vector3.Distance(transform.position, _positions[positionIndex]) < 0.01f)
            positionIndex++;
        
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

    public virtual void OnTargetReached()
    {
        Destroy(this.gameObject);
    }


    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (!other.gameObject.GetComponent<Unit>() || !other.gameObject.GetComponent<Projectile>())
    //     {
    //         Destroy(this.gameObject);
    //     }
    // }
}
