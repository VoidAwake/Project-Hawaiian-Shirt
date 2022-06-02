using System;
using Hawaiian.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class ChestSpawner : Spawner
{
    [SerializeField] private GameObject DroppedItem;
    [SerializeField] private Image _spawnTimerIndicator;

    private Animator _chestAnimator;

    public void Awake()
    {
        _chestAnimator = GetComponent<Animator>();
        _chestAnimator.SetBool("isOpen",false);
        currentSpawnTime = _spawnRate;
    }

    public override void SpawnItem()
    {
        _currentItem = _pool.RetrieveRandomItem();
        _currentItemInstance = Instantiate(DroppedItem, transform.localPosition, Quaternion.identity);
        DroppedItem droppedItem = _currentItemInstance.GetComponent<DroppedItem>();
        droppedItem.item = _currentItem;
    }

    protected override void TimerCompleted()
    {
        _spawnTimerIndicator.color = new Color32(102, 255, 103, 204);
        _chestAnimator.SetBool("isOpen", true);

    }

    protected override void Update()
    {
        base.Update();

        if (_currentItemInstance == null)
        {
          //  _chestAnimator.SetBool("isOpen", false);
            _spawnTimerIndicator.color = new Color32(255, 67, 64, 204);
        }

        _spawnTimerIndicator.fillAmount = 1 - (currentSpawnTime / _spawnRate);

    }

    public void OnAnimationComplete()
    {
        SpawnItem();
    }
    
    
}
