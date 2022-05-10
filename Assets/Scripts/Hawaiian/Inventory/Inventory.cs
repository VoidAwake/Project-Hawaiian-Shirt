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
        public UnityEvent itemchange = new ();
        public int invPosition = 0;
    
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
            /*if(inv.Count < invSize)
        {
            
        }*/

            for (int i = 0; i < inv.Length; i++)
            {
                if (inv[i] == null)
                {
                    inv[i] = item;
                    //item.Picked();??Delete item in player script as there is no reference to ingame item referring to scriptable object obtainable here
                    itemchange.Invoke();
                    Debug.Log("I have inserted an item into my inventory");
                    return true;
                }
            }

            return false;
        }


        public void DropItem(int invPosition)
        {
            //Manager.drop(inv[invPos]);
            inv[invPosition] = null;
            itemchange.Invoke();
        }

        /*public void OnUse(int invPos)
    {
        
    }*/
    
    }
}