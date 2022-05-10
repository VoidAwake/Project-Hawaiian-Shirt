using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hawaiian.Inventory
{

    public enum ItemType
    {
        Melee,
        Projectile,
        Throwable,
        Objective,
        Other,
    }
    
    
    [CreateAssetMenu]
    public class Item : ScriptableObject, IItem
    {
        [Header("Item Type")]
        public ItemType Type;

        //Main Components
        [HideInInspector] public Sprite ItemSprite;
        [HideInInspector] public Sprite DroppedItemSprite;
        [HideInInspector] public string ItemName;
        [HideInInspector] public int MaxStack;
        
        [HideInInspector] public GameObject DroppedItemBase;
        [HideInInspector] public GameObject ProjectileInstance;

        
        [HideInInspector] public float ItemDamage;

        //Projectile/Throwable Specific Stats
        [HideInInspector] public float DrawSpeed;
        [HideInInspector] public float DrawDistance;
        [HideInInspector] public bool SticksOnWall;

        //Melee Specific Stats
        [HideInInspector] public float AttackRate;
        
        //Objective Specific Stats
        [HideInInspector] public bool IsMainObjective;
        [HideInInspector] public bool CanBeHeldByEnemies;
        [HideInInspector] public float Points;

        //Other Specific Stats
        [HideInInspector] public bool IsKey;


        // Start is called before the first frame update

        public void DropItem() {}

        public void UseItem() {}

        public void UseItemAlternate() {}

        public float GetDamage() => ItemDamage;
        
    }
}