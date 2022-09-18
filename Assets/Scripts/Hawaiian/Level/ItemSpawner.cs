using Hawaiian.Inventory;
using UnityEngine;

namespace Hawaiian.Level
{
    public class ItemSpawner : Spawner
    {
        [SerializeField] private GameObject DroppedItem;
        [SerializeField] private float SpawnRate => _spawnRate;
    
        // Start is called before the first frame update
        void Start()
        {
            SpawnItem();
            currentSpawnTime = SpawnRate;
        }


        public override void SpawnItem()
        {
            _currentItem = _pool.RetrieveRandomItem();
            _currentItemInstance = Instantiate(DroppedItem, transform);
            DroppedItem droppedItem = _currentItemInstance.GetComponent<DroppedItem>();
            droppedItem.Item = _currentItem;
            currentSpawnTime = _spawnRate;

        }
    }
}
