using System;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public abstract class Spawner : MonoBehaviour
    {
        [SerializeField]  protected float _spawnRate;
        [SerializeField]  protected float currentSpawnTime;
         [SerializeField] protected ItemPool _pool;
        protected Item _currentItem;

        [SerializeField] protected GameObject _currentItemInstance;

        public abstract void SpawnItem();

        protected virtual void TimerCompleted()
        {
            SpawnItem();
        }

        protected virtual void Update()
        {
            if (_currentItemInstance == null)
            {
                if (currentSpawnTime >= 0.0f)
                    currentSpawnTime -= Time.deltaTime;
                else
                {
                    TimerCompleted();
                }
            }
        }
    }
}