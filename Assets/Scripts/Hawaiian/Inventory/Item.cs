using System;
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
        Trap,
        Shield
    }

    public enum ItemRarity
    {
        Common,
        Rare,
        VeryRare,
        Legendary
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
        // TODO: Misleading name
        [HideInInspector] public GameObject ProjectileInstance;
        [HideInInspector] public ItemRarity Rarity;
        [HideInInspector] public int ItemWeight;
        [HideInInspector] public GameObject heldItemPrefab;

        [HideInInspector] public int ProbabilityRangeFrom;
        [HideInInspector] public int ProbabilityRangeTo;

        [HideInInspector] public float ItemDamage;

        //Projectile/Throwable Specific Stats
        [HideInInspector] public float DrawSpeed;
        [HideInInspector] public float DrawDistance;
        [HideInInspector] public bool SticksOnWall;
        [HideInInspector] public bool ReturnsToPlayer;
        [HideInInspector] public bool IsMultiShot;
        [HideInInspector] public int ProjectileAmount;
        [HideInInspector] public bool IsRicochet;
        [HideInInspector] public int MaximumBounces;

        //Shield Specific Stats
        [HideInInspector] public float ParryWindow;
        [HideInInspector] public int ParryPercentageUpperLimit;
        [HideInInspector] public int ParryPercentageLowerLimit;
        [HideInInspector] public float TimeTillParry;
        [HideInInspector] public Sprite ShieldUp;
        [HideInInspector] public Sprite ShieldDown;


        //Melee Specific Stats
        [HideInInspector] public float AttackRate;
        [HideInInspector] public int KnockbackDistance;

        //Objective Specific Stats
        [HideInInspector] public bool IsMainObjective;
        [HideInInspector] public bool CanBeHeldByEnemies;
        [HideInInspector] public float Points;
        
        //Trap
        [HideInInspector] public int PlacementRadius;
        [HideInInspector] public Sprite PlacementIcon;
        [HideInInspector] public GameObject TrapInstance;


        //Other Specific Stats
        [HideInInspector] public bool IsKey;
        [HideInInspector] public bool IsDetonator;
        [HideInInspector] public bool IsDepositor;


        // Start is called before the first frame update

        public void DropItem() {}

        public void UseItem() {}

        public void UseItemAlternate() {}

        public float GetDamage() => ItemDamage;


        public void OnValidate()
        {
            switch (Rarity)
            {
                case ItemRarity.Common:
                    ItemWeight = 60;
                    break;
                case ItemRarity.Rare:
                    ItemWeight = 50;
                    break;
                case ItemRarity.VeryRare:
                    ItemWeight = 30;
                    break;
                case ItemRarity.Legendary:
                    ItemWeight = 10;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}