using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    // Start is called before the first frame update
    public string itemName;
    public float itemDamage;
    public Sprite itemSprite;
    public float maxStack;

    
}