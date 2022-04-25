using UnityEngine;

namespace Hawaiian.Inventory
{
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        public string itemName;
        public float itemDamage;
        public Sprite itemSprite;
        public float maxStack;
        public int score;
    }
}