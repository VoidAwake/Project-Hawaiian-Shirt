    using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Hawaiian.Inventory
{
    public class PlayerTreasure : MonoBehaviour
    {
        [SerializeField] private GameObject _bombEffectPrefab;
        [SerializeField] private GameObject _defuserIndicator;
        [SerializeField] private Slider _defuserSlider;
        [SerializeField] private float defuseTarget;
        [SerializeField] private float defuseTimer;
        [SerializeField] private GameObject DroppedItemPrefab;
        [SerializeField] private PlayerColors playerColors;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public List<Item> _items = new List<Item>();

        public Detonator DetonatorReference;
        public InventoryController playerInventoryController { get; set; }

        public bool IsBeingDetonated;
       

        private float _currentDetonationTime;

        private float _detonationTime;

        public UnityEvent pointsChanged = new();

        public int CurrentPoints
        {
            get => currentPoints;
            set
            {
                currentPoints = value;
                pointsChanged.Invoke();
            }
        }

        public Coroutine DefuserCoroutine;


        private bool flag = false;

        private int currentPoints;
        //   public GameEvent 

        public void Initialise(int playerNumber, InventoryController playerInventoryController)
        {
            spriteRenderer.color = playerColors.GetColor(playerNumber);

            this.playerInventoryController = playerInventoryController;
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.GetComponentInChildren<InventoryController>() != playerInventoryController)
                return;

            if (!IsBeingDetonated)
            {
                _defuserIndicator.SetActive(false);
                return;
            }

            _defuserIndicator.SetActive(true);

            if (DefuserCoroutine != null)
            {
                defuseTarget = 0;
                StopCoroutine(DefuserCoroutine);
            }
            else
                defuseTarget = 1;

            DefuserCoroutine = StartCoroutine(DefuseBombCoroutine(defuseTarget));
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.GetComponentInChildren<InventoryController>() != playerInventoryController)
                return;

            if (!IsBeingDetonated)
            {
                _defuserIndicator.SetActive(false);
                return;
            }

            if (!flag)
                flag = true;
            else
                return;


            _defuserIndicator.SetActive(true);

            if (DefuserCoroutine != null)
            {
                defuseTarget = 0;
                StopCoroutine(DefuserCoroutine);
            }
            else
                defuseTarget = 1;

            DefuserCoroutine = StartCoroutine(DefuseBombCoroutine(defuseTarget));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.GetComponentInChildren<InventoryController>() != playerInventoryController)
                return;

            if (!IsBeingDetonated)
            {
                _defuserIndicator.SetActive(false);
                return;
            }


            _defuserIndicator.SetActive(true);
            defuseTarget = 0;

            if (DefuserCoroutine != null)
                StopCoroutine(DefuserCoroutine);

            DefuserCoroutine = StartCoroutine(DefuseBombCoroutine(defuseTarget));
        }


        IEnumerator DefuseBombCoroutine(float target)
        {
            float elapsedTime = _defuserSlider.value;

            if (target > 0)
            {
                while (elapsedTime < 1)
                {
                    elapsedTime += Time.deltaTime;
                    _defuserSlider.value = (elapsedTime / 1);
                    yield return null;
                }

                Destroy(DetonatorReference.gameObject);
                IsBeingDetonated = false;
                defuseTimer = 0;
                _defuserSlider.value = 0;
            }
            else
            {
                while (elapsedTime > 0)
                {
                    elapsedTime -= Time.deltaTime;
                    _defuserSlider.value = (1 - elapsedTime / 1);
                    yield return null;
                }

                defuseTimer = 0;
                _defuserSlider.value = 0;
            }

            flag = false;
            _defuserIndicator.SetActive(false);
            DefuserCoroutine = null;
        }


        public void AddTreasure()
        {
            Hawaiian.Inventory.Inventory inventory = ScriptableObject.CreateInstance<Hawaiian.Inventory.Inventory>();

            inventory = playerInventoryController.inv;

            int newPoints = 0;
            List<int> newItemsIndexes = new List<int>();

            for (var index = 0; index < inventory.inv.Length; index++)
            {
                Item item = inventory.inv[index];

                if (item != null)
                {
                    if (item.Type == ItemType.Objective)
                    {
                        newPoints += (int) item.Points;
                        newItemsIndexes.Add(index);
                        _items.Add(item);
                    }
                }
            }


            for (var i = 0; i < inventory.inv.Length; i++)
            {
                foreach (var t in newItemsIndexes)
                {
                    if (t == i)
                        inventory.RemoveItemAt(i);
                }
            }


            CurrentPoints += newPoints;
        }

        public void SetIsBeingDetonated()
        {

            IsBeingDetonated = true;
        }


        public void DetonateBase()
        {
            if (!IsBeingDetonated)
                return;

            Instantiate(_bombEffectPrefab, transform.position, Quaternion.identity);

            var temp = new List<Item>();
            int pointsToRemove = 0;

            for (var index = 0; index < 3; index++)
            {
                if (index >= _items.Count)
                    break;

                Item item = _items[index];
                temp.Add(item);
                pointsToRemove += (int) item.Points;
                GameObject droppedItem = Instantiate(DroppedItemPrefab, transform.position, Quaternion.identity);
                droppedItem.GetComponent<DroppedItem>().Item = item;

                var randomX = UnityEngine.Random.Range(-1f, 1f);
                var randomY = UnityEngine.Random.Range(-1f, 1f);
                var randomLength = UnityEngine.Random.Range(2f, 4f);

                var direction = new Vector2(randomX, randomY);
                direction = direction * randomLength;
                droppedItem.GetComponent<ItemUnit>().OnThrow(direction);
            }


            CurrentPoints -= pointsToRemove;


            _items = _items.Except(temp).ToList();
            IsBeingDetonated = false;
        }

        public void GetDetonatorReference(Tuple<IUnit, Detonator> reference)
        {
            if (reference.Item1 == playerInventoryController)
                return;

            if (DetonatorReference != null)
            {
                Destroy(reference.Item2);
                return;
            }
            
            DetonatorReference = reference.Item2;
            DetonatorReference.Treasure = this;
            IsBeingDetonated = true;   

        }
    }
}