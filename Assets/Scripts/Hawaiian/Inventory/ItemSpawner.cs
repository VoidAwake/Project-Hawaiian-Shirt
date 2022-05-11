using System.Collections;
using System.Collections.Generic;
using Hawaiian.Inventory;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    [SerializeField] private GameObject DroppedItem;
    [SerializeField] private Item _currentItem;
    [SerializeField] private ItemPool _pool;
    [SerializeField] private float SpawnRate;
    [SerializeField] private float currentSpawnTime;

    [SerializeField] private GameObject _currentItemInstance;
    
    // Start is called before the first frame update
    void Start()
    {
      SpawnItem();
      currentSpawnTime = SpawnRate;
    }

    public void SpawnItem()
    {
        _currentItem = _pool.RetrieveRandomItem();
        _currentItemInstance = Instantiate(DroppedItem, transform.localPosition = Vector3.zero, quaternion.identity);
        DroppedItem droppedItem = _currentItemInstance.GetComponent<DroppedItem>();
        droppedItem.item = _currentItem;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentItemInstance == null)
        {
            if (currentSpawnTime >= 0.0f)
                currentSpawnTime -= Time.deltaTime;
            else
            {
                SpawnItem();
                currentSpawnTime = SpawnRate;
            }
        }
    }
}
