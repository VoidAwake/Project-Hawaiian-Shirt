using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Unit
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private bool _canStickOnWalls;
        [SerializeField] private float _damage;

        private float _currentDistance;
        private Vector2 _targetLocation;

        public float Speed => _speed;
        public bool CanStickOnWalls => _canStickOnWalls;
        public float Damage => _damage;

        public void Initialise(Vector3 target)
        {
            _targetLocation = target;
        }
        
        public void Initialise(Vector3 target, float speed = 5, float damage = 5,bool canStickOnWalls = false)
        {
            _targetLocation = target;
            _speed = speed;
            _damage = damage;
            _canStickOnWalls = canStickOnWalls;
        }
        

        private void Update()
        {
            var step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetLocation, step);

            if (Vector3.Distance(transform.position, _targetLocation) < 0.01f)
            {
               Destroy(this.gameObject);
            }
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.GetComponent<Unit>() || !other.gameObject.GetComponent<Projectile>())
            {
                Destroy(this.gameObject);
            }
        }
    }

}