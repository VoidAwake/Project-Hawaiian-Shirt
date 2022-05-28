using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.InputSystem.UI;

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


   

    public override void Initialise(Vector2 [] targets, Sprite newSprite,bool canStickOnWalls = false)
    {
        _positions = targets;
        _canStickOnWalls = canStickOnWalls;
        positionIndex = 0;
        _renderer.sprite = newSprite;
    }

    public void UpdateTargetToFinalDestination(Vector2 direction)
    {
        _positions = null;
        StartCoroutine(LerpPositionToDestination(direction));
    }

    IEnumerator LerpPositionToDestination(Vector2 finalPosition)
    {
        List<Vector2> positions = _positions.ToList();
        Vector2 startingPosition = transform.position;
        Vector2 endPosition = finalPosition;

        while (!startingPosition.Equals(endPosition))
        {
            startingPosition = Vector2.Lerp(startingPosition, endPosition, Time.deltaTime);
            positions.Add(startingPosition);
            yield return null;
        }

        _positions = positions.ToArray();
    }

    private void Awake()
    {
        positionIndex = 0;
        _renderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Update()
    {
        if (_positions == null)
            return;

        if (positionIndex >= _positions.Length - 1)
            OnTargetReached();
        
        var step = _speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, _positions[positionIndex], step);
        
        if (Vector3.Distance(transform.position, _positions[positionIndex]) < 0.01f)
            positionIndex++;
        
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
