using Hawaiian.PositionalEvents;
using Hawaiian.Unit;
using Hawaiian.Utilities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

namespace Hawaiian.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] public UnitPlayer _player;
        [SerializeField] private GameEvent parse;
        [SerializeField] private bool addinv;
        [SerializeField] private Item item;
        [SerializeField] private ScriptableInt size;

        [SerializeField] private BaseGameEvent<Inventory> addedInventory;

        [SerializeField] private SpriteRenderer hand;

        [SerializeField] private GameObject droppedItem;

        public UnityEvent currentItemChanged = new UnityEvent();

        private int tempPos;

        private int prevScore = 0;
       
        //[SerializeField] private int invSize;a


        public Inventory _inv;
        private PositionalEventCaller positionalEventCaller;

        public Item CurrentItem => _inv.CurrentItem;
        
        public float Score => _inv.Score;
    
        private void Awake()
        {
            _inv = ScriptableObject.CreateInstance<Inventory>();
            _inv.SetInventory(size.Value);
            _inv.currentItemChanged.AddListener(OnCurrentItemChanged);
            _inv.currentItemChanged.AddListener(CreateScorePopUp);
            
            addedInventory.Raise(_inv);
            _player = GetComponentInParent<UnitPlayer>();
          
            addinv = false;
            positionalEventCaller = GetComponent<PositionalEventCaller>();
        }

        private void OnCurrentItemChanged()
        {
            currentItemChanged.Invoke();
        }

        // TODO: Replace with messages
        private void OnEnable()
        {
            _player.GetPlayerInput().actions["InvParse"].performed += SwitchItem;
        }

        private void OnDisable()
        {
            _player.GetPlayerInput().actions["InvParse"].performed -= SwitchItem;
        }

        public void InitialiseHighlight()
        {
            if (!addinv) return;
            
            _inv.AddItem(item);
            addinv = !addinv;
        }

        private void OnPickUp()
        {
            if (_player.playerState.Equals(Unit.Unit.PlayerState.Tripped)) return;
            
            foreach (var target in positionalEventCaller.Targets)
            {
                var item = target.GetComponent<DroppedItem>().item;

                if (item == null) continue;

                if (!_inv.AddItem(item)) continue;

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
            if (GetComponent<ItemInteractor>().IsAttacking) // makes sure that they cant change their items while attacking since that make it go brokey
                return;
            
            _inv.InvPosition++;
            Parse();
        }

        public void OnCycleBackward()
        {
            // TODO: Two way dependency.
            // TODO: Replace with a cancel attack function
            if (GetComponent<ItemInteractor>().IsAttacking) // makes sure that they cant change their items while attacking since that make it go brokey
                return;
            
            _inv.InvPosition--;
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
            // TODO: Two way dependency.
            // TODO: Replace with a cancel attack function
            if (GetComponent<ItemInteractor>().IsAttacking) // makes sure that they cant change their items while attacking since that make it go brokey
                return;
            _inv.invPosition = x;
            Parse();
        }
        
        private void Parse()
        {
            
             //_inv.invPosition += i;
            if (_inv.InvPosition > _inv.inv.Length - 1)
            {
                _inv.InvPosition = 0;
            }

            if (_inv.InvPosition < 0)
            {
                _inv.InvPosition = _inv.inv.Length - 1;
            }
            //SelectionUpdate();
            if (_inv.inv[_inv.InvPosition] != null)
            {
                hand.sprite = _inv.inv[_inv.InvPosition].ItemSprite;
                
            }
            else
            {
                hand.sprite = null;
            }
            
            //how do i call an event c:

            currentItemChanged.Invoke();

            parse.Raise();
            
        }
        
        public void DropItLikeItsHot(Vector2 rad)
        {
            DropItem(_inv.invPosition, rad);
        }

        public void RemoveCurrentItem()
        {
            RemoveItemFromIndex(_inv.InvPosition);
        }


        public void DropRandom(Vector2 dir)
        {
            var itemIndexes = new List<int>();

            for (int i = 0; i < _inv.inv.Length; i++)
            {
                if (_inv.inv[i] != null)
                    itemIndexes.Add(i);
            }

            if (itemIndexes.Count == 0) return;

            var randomItemIndex = itemIndexes[UnityEngine.Random.Range(0, itemIndexes.Count)];

            DropItem(randomItemIndex, dir);
        }

        private void DropItem(int invPosition, Vector2 dir)
        {
            if (_inv.inv[invPosition] != null)
            {
                GameObject dp = Instantiate(droppedItem, transform.position, quaternion.identity);
                dp.GetComponent<DroppedItem>().item = _inv.inv[invPosition];
                dp.GetComponent<SpriteRenderer>().sprite = _inv.inv[invPosition].DroppedItemSprite;
                dp.GetComponent<ItemUnit>().OnThrow(dir);
                _inv.RemoveItemAt(invPosition);
                hand.sprite = null;
            }
            else
            {
                Debug.Log("THIS BITCH EMPTY...............................YEET");
            }
        }

        public void RemoveItemFromIndex(int invPosition)
        {
            if (_inv.inv[invPosition] == null) return;
            
            _inv.RemoveItemAt(_inv.InvPosition);
            hand.sprite = null;
        }

        public void UseItem()
        {
            
        }

        private void SelectionUpdate()
        {
            
        }

        private void CreateScorePopUp()
        {
            int newScore = (int)_inv.inv.Where(i => i != null).Sum(i => i.Points);

            if (newScore != prevScore)
            {
                FindObjectOfType<ScorePopUpManager>().InstantiateScorePopUp(transform.parent, newScore - prevScore);
            }

            prevScore = newScore;
        }
    }
}
