using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Codice.Client.BaseCommands;
using Codice.CM.WorkspaceServer.Tree;
using Hawaiian.Inventory;
using Hawaiian.Utilities;
using PlasticGui.Configuration.CloudEdition.Welcome;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Hawaiian.Unit
{
    public class PlayerTreasure : MonoBehaviour
    {
        [SerializeField] private GameObject _bombEffectPrefab;
        [SerializeField] private GameObject _defuserIndicator;
        [SerializeField] private Slider _defuserSlider;

        [SerializeField] private float defuseTarget;
        [SerializeField] private float defuseTimer;


        [SerializeField] private GameObject DroppedItemPrefab;
        public List<Item> _items = new List<Item>();

        public Detonator DetonatorReference;
        public IUnit PlayerReference { get; set; }
        [SerializeField] private IUnitIntGameEvent UpdatePoint;

        public IUnitIntGameEvent AddPoints;

        public bool IsBeingDetonated;

        private float _currentDetonationTime;

        private float _detonationTime;

        public int CurrentPoints;

        public Coroutine DefuserCoroutine;


        private bool flag = false;
        //   public GameEvent 

        public void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.GetComponent<IUnit>() != PlayerReference)
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
            if (other.gameObject.GetComponent<IUnit>() != PlayerReference)
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
            if (other.gameObject.GetComponent<IUnit>() != PlayerReference)
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
            Inventory.Inventory inventory = ScriptableObject.CreateInstance<Inventory.Inventory>();

            for (int i = 0; i < PlayerReference.GetUnit().transform.childCount; i++)
            {
                InventoryController temp = PlayerReference.GetUnit().transform.GetChild(i)
                    .GetComponent<InventoryController>();

                if (temp == null) continue;

                inventory = temp._inv;
            }

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
            Tuple<IUnit, int> reference = new Tuple<IUnit, int>(PlayerReference, CurrentPoints);
            AddPoints.Raise(reference);
        }

        public void SetIsBeingDetonated()
        {
            if (IsBeingDetonated == true)
            {
                
            }
            
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
                droppedItem.GetComponent<DroppedItem>().item = item;

                var randomX = UnityEngine.Random.Range(-1f, 1f);
                var randomY = UnityEngine.Random.Range(-1f, 1f);
                var randomLength = UnityEngine.Random.Range(2f, 4f);

                var direction = new Vector2(randomX, randomY);
                direction = direction * randomLength;
                droppedItem.GetComponent<ItemUnit>().OnThrow(direction);
            }


            CurrentPoints -= pointsToRemove;


            _items = _items.Except(temp).ToList();
            Tuple<IUnit, int> reference = new Tuple<IUnit, int>(PlayerReference, CurrentPoints);
            UpdatePoint.Raise(reference);
            IsBeingDetonated = false;
        }

        public void GetDetonatorReference(Tuple<IUnit, Detonator> reference)
        {
            if (reference.Item1 == PlayerReference)
                return;

            if (DetonatorReference != null)
            {
                Destroy(reference.Item2);
                return;
            }
            
            DetonatorReference = reference.Item2;
            DetonatorReference.Treasure = this;
        }
    }
}