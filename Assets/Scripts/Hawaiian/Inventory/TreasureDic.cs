using System.Collections;
using System.Collections.Generic;
using Hawaiian.Inventory;
using UnityEngine;

[CreateAssetMenu]
public class TreasureDic : ScriptableObject
{

    [SerializeField] private Item[] itemReference;

    // Start is called before the first frame update
    void SortReference()
    {
        for (int i = 0; i < itemReference.Length - 1; i++)
        {
            for (int x = i; x < itemReference.Length - 1; x++)
            {
                if (itemReference[x].Points > itemReference[x + 1].Points)
                {
                    (itemReference[x + 1], itemReference[x]) = (itemReference[x], itemReference[x + 1]);
                }
            }
        }
    }
    
    public Item[] GetTreasuresToDrop(int amountToDrop)
    {
        List<Item> toSpawn = new List<Item>();
        //inv.score -= amountToDrop;
        int x = itemReference.Length - 1;
        while (amountToDrop > 0)
        {
            while ((int)itemReference[x].Points > amountToDrop) {x--;}
            int treasurePiece = (int)Mathf.Floor(Random.Range(0, x+1));
            amountToDrop -= (int)itemReference[treasurePiece].Points;
            toSpawn.Add(itemReference[treasurePiece]);
        }

        Item[] arraySpawn = toSpawn.ToArray();
        return arraySpawn;
    }
}
