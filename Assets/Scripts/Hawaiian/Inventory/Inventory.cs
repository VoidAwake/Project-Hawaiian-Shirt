using UnityEngine;
using UnityEngine.Events;
using Hawaiian.Utilities;

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
        private int invPosition = 0;

        public int InvPosition
        {
            get => invPosition;
            set
            {
                invPosition = value;
                currentItemChanged.Invoke();
            }
        }

        public Item CurrentItem => inv[InvPosition];

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