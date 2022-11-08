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

        public void RemoveItemAt(int invPosition)
        {
            inv[invPosition] = null;
            inventoryChanged.Invoke();
        }
    }
}