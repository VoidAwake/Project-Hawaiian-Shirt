using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Hawaiian.Inventory;
using MoreLinq;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class ItemPool
{

    [SerializeField] private Item[] _pool;

    public Item[] Pool => _pool;

    public int GetTotalWeight()
    {
        int totalWeight = 0;
        _pool.ForEach(x => totalWeight += x.ItemWeight);
        return totalWeight;
    }

    public void ValidatePool()
    {

        var currentProbabilityWeight = 0;
        
        foreach (Item item in Pool)
        {
            item.ProbabilityRangeFrom = currentProbabilityWeight;
            currentProbabilityWeight += item.ItemWeight;	
            item.ProbabilityRangeTo = currentProbabilityWeight;						
        }
    }

    public Item RetrieveRandomItem()
    {

        ValidatePool();
        var number = UnityEngine. Random.Range(0, GetTotalWeight());
        
        foreach (Item item in _pool)
        {
            // If the picked number matches the item's range, return item
            if(number > item.ProbabilityRangeFrom && number < item.ProbabilityRangeTo)
                return item;
        }

        return _pool[0];
    }

  
    
}
