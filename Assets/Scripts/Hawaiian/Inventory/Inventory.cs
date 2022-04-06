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
        public UnityEvent itemchange = new ();
    
        public Inventory()
        {
            inv = new Item[3];
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
                    return true;
                }
            }

            return false;
        }


        public void DropItem(Vector2 pos, int invPos)
        {
            //Manager.drop(inv[invPos]);
            inv[invPos] = null;
            itemchange.Invoke();
        }

        /*public void OnUse(int invPos)
    {
        
    }*/
    
    }
}