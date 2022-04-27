using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    
    [SerializeField] private float _speed;
    [SerializeField] private bool _canStickOnWalls;
    [SerializeField] private float _damage;

    private SpriteRenderer _renderer;
    private float _currentDistance;
    private Vector2[] _positions;
    private int positionIndex;
    
    public float Speed => _speed;
    public bool CanStickOnWalls => _canStickOnWalls;
    public float Damage => _damage;

        
    public void Initialise(Vector2 [] targets, Sprite newSprite,float speed = 5, float damage = 5,bool canStickOnWalls = false)
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

    private void Update()
    {
        if (_positions == null)
            return;
        
        if (positionIndex >= _positions.Length - 1)
            Destroy(this.gameObject);

        var step = _speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, _positions[positionIndex], step);
        
        if (Vector3.Distance(transform.position, _positions[positionIndex]) < 0.01f)
            positionIndex++;
        
    }


    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (!other.gameObject.GetComponent<Unit>() || !other.gameObject.GetComponent<Projectile>())
    //     {
    //         Destroy(this.gameObject);
    //     }
    // }
}
