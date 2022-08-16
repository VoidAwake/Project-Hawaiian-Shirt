using System;
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

    private bool hasCollided;
    
    public float Speed => _speed;
    public bool CanStickOnWalls => _canStickOnWalls;
    public Vector2 LastPosition
    {
        get => _positions[^1]; // ^1 
        set => _positions[^1] = value;
    }


    public override void Initialise(Vector2 [] targets, Sprite newSprite,bool canStickOnWalls = false)
    {
        hasCollided = false;
        _positions = targets;
        _canStickOnWalls = canStickOnWalls;
        positionIndex = 0;
        _renderer.sprite = newSprite;
    }

    public void UpdateTargetToFinalDestination(Vector2 direction)
    {
        _positions = _positions.Where((position, index) => index < positionIndex).ToArray();
        _positions = _positions.Reverse().ToArray();
        positionIndex = 0;
    }

    IEnumerator LerpPositionToDestination(Vector2 finalPosition)
    {
        List<Vector2> positions = _positions.ToList();
        Vector2 startingPosition = transform.position;
        Vector2 endPosition = finalPosition;

        while (!startingPosition.Equals(endPosition))
        {
            startingPosition = Vector2.Lerp(startingPosition, endPosition,_speed);
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
        if (hasCollided)
        {
            LastPosition = transform.position;
            OnTargetReached();
            return;
        }
        
        if (_positions == null)
            return;

        if (positionIndex >= _positions.Length - 1)
            OnTargetReached();
        
        var step = _speed;

        transform.position = Vector3.MoveTowards(transform.position, _positions[positionIndex], step);
        
        if (Vector3.Distance(transform.position, _positions[positionIndex]) < 0.01f)
            positionIndex++;
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<UnitPlayer>()) return;
        if (other.gameObject.GetComponent<Projectile>()) return;
        if (other.gameObject.GetComponent<ItemUnit>()) return;
        if (other.gameObject.GetComponent<ShieldCollider>()) return;
        if (other.gameObject.GetComponent<AvoidHit>()) return;

        Debug.Log(!other.gameObject.GetComponent<UnitPlayer>() + " state of the unit player");
        Debug.Log(!other.gameObject.GetComponent<Projectile>() + " state of the projectile");
        Debug.Log(!other.gameObject.GetComponent<ItemUnit>() + " state of the item unit");
        hasCollided = true;
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
