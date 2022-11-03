using Hawaiian.PositionalEvents;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using Random = UnityEngine.Random;

namespace Hawaiian.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] public UnitPlayer player;
        [SerializeField] private GameEvent parse;
        [SerializeField] private ScriptableInt size;
        [SerializeField] private SpriteRenderer hand;
        [SerializeField] private GameObject droppedItemPrefab;
        [SerializeField] private Cursor cursor;
        [SerializeField] private int[] bitValues = new int[]{1,2,4,8,16,32,64,128};
        [SerializeField] private TreasureDic dictionary;
        

        public UnityEvent currentItemChanged = new UnityEvent();
        public Inventory inv;

        private int tempPos;
        private int prevScore = 0;
        private PositionalEventCaller positionalEventCaller;

        public Item CurrentItem => inv.CurrentItem;
        public float Score => inv.Score;

        private void Awake()
        {
            inv = ScriptableObject.CreateInstance<Inventory>();
            inv.SetInventory(size.Value);
            inv.currentItemChanged.AddListener(OnCurrentItemChanged);
            inv.currentItemChanged.AddListener(CreateScorePopUp);

            player = GetComponentInParent<UnitPlayer>();

            positionalEventCaller = GetComponent<PositionalEventCaller>();
        }

        private void OnCurrentItemChanged()
        {
            currentItemChanged.Invoke();
            player.IsSlowed = false;
        }

        // TODO: Replace with messages
        private void OnEnable()
        {
            player.GetPlayerInput().actions["InvParse"].performed += SwitchItem;
        }

        private void OnDisable()
        {
            player.GetPlayerInput().actions["InvParse"].performed -= SwitchItem;
        }

        private void OnPickUp()
        {
            if (player.playerState.Equals(Unit.Unit.PlayerState.Tripped)) return;

            foreach (var target in positionalEventCaller.Targets)
            {
                var item = target.GetComponent<DroppedItem>().Item;

                if (item == null) continue;

                if (!inv.AddItem(item)) continue;

                positionalEventCaller.Raise(target);
            }
        }

        public void SwitchItem(InputAction.CallbackContext value)
        {
            if (!value.performed)
                return;

            if (value.ReadValue<float>() > 0f)
                OnCycleBackward();
            else
                OnCycleForward();
        }

        public void OnCycleForward()
        {
            // TODO: Two way dependency.
            // TODO: Replace with a cancel attack function
            // if (GetComponent<ItemInteractor>()
            //     .IsAttacking) // makes sure that they cant change their items while attacking since that make it go brokey
            //     return;

            inv.InvPosition++;
            Parse();
        }

        public void OnCycleBackward()
        {
            // TODO: Two way dependency.
            // TODO: Replace with a cancel attack function
            // if (GetComponent<ItemInteractor>()
            //     .IsAttacking) // makes sure that they cant change their items while attacking since that make it go brokey
            //     return;

            inv.InvPosition--;
            Parse();
        }

        public void OnParseOne()
        {
            OnNumParse(0);
        }

        public void OnParseTwo()
        {
            OnNumParse(1);
        }

        public void OnParseThree()
        {
            OnNumParse(2);
        }

        public void OnParseFour()
        {
            OnNumParse(3);
        }

        public void OnParseFive()
        {
            OnNumParse(4);
        }

        public void OnNumParse(int x)
        {
            // // TODO: Two way dependency.
            // // TODO: Replace with a cancel attack function
            // if (GetComponent<ItemInteractor>()
            //     .IsAttacking) // makes sure that they cant change their items while attacking since that make it go brokey
            //     return;
            inv.invPosition = x;
            Parse();
        }

        private void Parse()
        {
            inv.invPosition = (int) Mathf.Repeat(inv.invPosition, inv.inv.Length);

            hand.sprite = inv.inv[inv.InvPosition]?.ItemSprite;

            currentItemChanged.Invoke();

            parse.Raise();
        }

        public void DropItLikeItsHot(Vector2 rad)
        {
            if (CurrentItem != null && CurrentItem.IsDepositor)
                return;

            DropItem(inv.invPosition, rad);
        }

        public void RemoveCurrentItem(IUnit unit)
        {
            if (player.PlayerNumber != unit.PlayerNumber)
                return;

            RemoveItemFromIndex(inv.InvPosition);
        }


        public void DropCash()
        {
            //List<int> toSpawn = new List<int>();
            //toSpawn = GetTreasuresToDrop((int)Mathf.Floor(inv.score * (30 + (inv.score/200 * 50))/100));
            int amountToDrop = ((int) Mathf.Floor(inv.score * (30 + (inv.score / 200 * 50)) / 100));
            Item[] toSpawn = dictionary.GetTreasuresToDrop(amountToDrop);
            foreach (Item reference in toSpawn)
            {
                Item treasure = ScriptableObject.CreateInstance<Item>();
                treasure.ItemName = reference.ItemName;
                treasure.ItemSprite = reference.ItemSprite;
                treasure.DroppedItemSprite = reference.DroppedItemSprite;
                treasure.Type = ItemType.Objective;
                treasure.Points = reference.Points;
                treasure.DroppedItemBase = reference.DroppedItemBase;

                GameObject droppedTreasure = Instantiate(reference.DroppedItemBase,transform.position, Quaternion.identity);
                droppedTreasure.GetComponent<DroppedItem>().Item = treasure;

                for (int i = 0; i < droppedTreasure.transform.childCount; i++)
                {
                    if (droppedTreasure.transform.GetChild(i).name == "Item Sprite")
                    {
                        droppedTreasure.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite =
                            treasure.ItemSprite;
                    }
                }
                
                droppedTreasure.GetComponent<ItemUnit>().OnThrow(Random.insideUnitCircle);
            }

            inv.LoseTreasure(amountToDrop);
        }


        private void DropTreasure(Item[] toSpawn)
        {
            
        }
        

        public void RemoveCurrentItem()
        {
            RemoveItemFromIndex(inv.InvPosition);
        }

        public void DropRandom(Vector2 dir)
        {
            var itemIndexes = new List<int>();

            for (int i = 0; i < inv.inv.Length; i++)
            {
                if (inv.inv[i] != null)
                {
                    if (!inv.inv[i].IsDepositor)
                        itemIndexes.Add(i);
                }
            }

            if (itemIndexes.Count == 0) return;

            var randomItemIndex = itemIndexes[UnityEngine.Random.Range(0, itemIndexes.Count)];


            DropItem(randomItemIndex, dir);
        }

        private void DropItem(int invPosition, Vector2 dir)
        {
            if (inv.inv[invPosition] == null) return;
            
            GameObject droppedItemObject = Instantiate(droppedItemPrefab, transform.position + Vector3.up * 0.5f, quaternion.identity);
            
            droppedItemObject.GetComponent<DroppedItem>().Item = inv.inv[invPosition];
            
            // I'm so sorry
            for (int i = 0; i < droppedItemObject.transform.childCount; i++)
            {
                if (transform.parent.GetChild(i).name == "Item Sprite")
                {
                    droppedItemObject.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite =
                        inv.inv[invPosition].DroppedItemSprite;
                }
            }
            
            droppedItemObject.GetComponent<ItemUnit>().OnThrow(dir);
            inv.RemoveItemAt(invPosition);
            hand.sprite = null;
        }

        public void RemoveItemFromIndex(int invPosition)
        {
            if (inv.inv[invPosition] == null) return;

            inv.RemoveItemAt(inv.InvPosition);
            hand.sprite = null;
        }

        private void CreateScorePopUp()
        {
            int newScore = (int) inv.inv.Where(i => i != null).Sum(i => i.Points);

            if (newScore != prevScore)
            {
                FindObjectOfType<ScorePopUpManager>().InstantiateScorePopUp(transform.parent, newScore - prevScore);
            }

            prevScore = newScore;
        }
        
        // TODO: Sort this out
        // TODO: Why is this here when all the other inputs are handled by InventoryController?
        public void OnDrop()
        {
            // // Prevent items being dropped while attacking
            // if (GetComponent<ItemInteractor>().IsAttacking) return;
        
            //_controller.DropItLikeItsHot(_rotation);
            Vector3 prevInput = (cursor.transform.localPosition - Vector3.up * 0.5f);
            Vector3 playerInput = prevInput.magnitude == 0 ? Vector2.right.normalized : prevInput.normalized;

            DropItLikeItsHot(playerInput);
        }
    }
}