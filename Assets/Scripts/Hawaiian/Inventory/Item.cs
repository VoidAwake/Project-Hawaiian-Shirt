using UnityEngine;

namespace Hawaiian.Inventory
{
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        // Start is called before the first frame update
        public string itemName;
        public float itemDamage;
        public Sprite itemSprite;
        public float maxStack;

    
    }
}