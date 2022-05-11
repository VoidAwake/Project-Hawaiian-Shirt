using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    
    [SerializeField] internal float _speed;
    [SerializeField] internal bool _canStickOnWalls;
    [SerializeField] internal float _damage;

    private SpriteRenderer _renderer;
    private float _currentDistance;
    private Vector2[] _positions;
    private int positionIndex;
    
    public float Speed => _speed;
    public bool CanStickOnWalls => _canStickOnWalls;
    public float Damage => _damage;
    public Vector2 LastPosition => _positions[^1]; // ^1 
    

        
    public virtual void Initialise(Vector2 [] targets, Sprite newSprite,float speed = 5, float damage = 5,bool canStickOnWalls = false)
    {
        _positions = targets;
        _speed = speed;
        _damage = damage;
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
