using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : ScriptableObject
{
    // Start is called before the first frame update

    [SerializeField] private static int invSize = 3;
    [SerializeField] private Item[] inv = new Item[invSize];


    public void PickUp(Item item)
    {
        /*if(inv.Count < invSize)
        {
            
        }*/

        for (int i = 0; i < invSize-1; i++)
        {
            if (inv[i] == null)
            {
                inv[i] = item;
                //item.Picked();??Delete item in player script as there is no reference to ingame item referring to scriptable object obtainable here
            }
        }
    }


    public void DropItem(Vector2 pos, int invPos)
    {
        //Manager.drop(inv[invPos]);
        inv[invPos] = null;
    }
}
