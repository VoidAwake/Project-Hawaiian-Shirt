using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField] private List<Item> Inv;


    /*public void Dropped(Vector2 pos)
    {
        
    }

    public void Picked()
    {
        
    }
    */
}
