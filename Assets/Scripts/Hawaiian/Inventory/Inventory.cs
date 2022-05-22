using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory
{
    [CreateAssetMenu]
    public class Inventory : ScriptableObject
    {
        // Start is called before the first frame update
    
        //[SerializeField] private ScriptableInt invSize;
        //[SerializeField] private int invSize;
        [SerializeField] public Item[] inv;
        [SerializeField] public int size;
        public UnityEvent currentItemChanged = new();
        public int invPosition = 0;

        public int InvPosition
        {
            get => invPosition;
            set
            {
                invPosition = value;
            }
        }

        public Item CurrentItem => inv[InvPosition];
        
        public float Score => inv.Where(i => i != null).Sum(i => i.Points);

        public Inventory()
        {
            //inv = new Item[size];
        }
        public void SetInventory(int invSize)
        {
            size = invSize;
            inv = new Item[size];
        }
        public bool PickUp(Item item)
        {
            for (int i = 0; i < inv.Length; i++)
            {
                if (inv[i] == null)
                {
                    inv[i] = item;
                    //item.Picked();??Delete item in player script as there is no reference to ingame item referring to scriptable object obtainable here
                    currentItemChanged.Invoke();
                    return true;
                }
            }

            return false;
        }

        // TODO: Rename. This is just removing an item, dropping is handled by InventoryController.
        public void DropItem(int invPosition)
        {
            //Manager.drop(inv[invPos]);
            inv[invPosition] = null;
            currentItemChanged.Invoke();
        }

        /*public void OnUse(int invPos)
    {
        
    }*/
    
    }
}