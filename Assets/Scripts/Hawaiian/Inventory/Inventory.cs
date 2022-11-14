using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory
{
    [CreateAssetMenu]
    public class Inventory : ScriptableObject
    {
        [SerializeField] public Item[] inv;
        [SerializeField] public int size;
        public UnityEvent inventoryChanged = new();
        public int invPosition = 0;

        public int InvPosition
        {
            get => invPosition;
            set => invPosition = value;
        }

        public Item CurrentItem => inv[InvPosition];
        
        //public float Score => inv.Where(i => i != null).Sum(i => i.Points);

        public void SetInventorySize(int invSize)
        {
            size = invSize;
            inv = new Item[size];
        }
        
        public bool AddItem(Item item)
        {
            for (int i = 0; i < inv.Length; i++)
            {
                if (inv[i] != null) continue;

                inv[i] = item;

                inventoryChanged.Invoke();
                return true;
            }

            return false;
        }

        
        //TODO: Remove upon inventory refactor
        public Dictionary<int,Item> GetAllTreasuresAndPositions()
        {
            Dictionary<int, Item> treasures = new Dictionary<int, Item>();
            
            

            for (var i = 0; i < inv.Length; i++)
            {
                if (inv[i] == null || inv[i].Type != ItemType.Objective)
                    continue;
                
                Item treasure = inv[i];
                treasures.Add(i, treasure);
            }

            return treasures;
        }

        public KeyValuePair<int, Item> GetMostLeftItem()
        {
            
            Dictionary<int, Item> treasures = GetAllTreasuresAndPositions();
            return treasures.First();
        }


        //TODO: Remove upon inventory refactor
        public IEnumerable<Item> GetAllTreasures()
        {
            IEnumerable<Item> treasures = inv.Where(i => i != null && i.Type == ItemType.Objective);
            return treasures.ToList();
        }

        public void RemoveItemAt(int invPosition)
        {
            inv[invPosition] = null;
            inventoryChanged.Invoke();
        }
    }
}