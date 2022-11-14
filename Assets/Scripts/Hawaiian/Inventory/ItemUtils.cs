using UnityEngine;

namespace Hawaiian.Inventory
{
    public static class ItemUtils
    {
        public static Item GenerateItem(string name, Sprite itemSprite, ItemType type, GameObject droppedItemReference, float points = 0f, Sprite droppedItemSprite = default )
        {
            Item generatedItem = ScriptableObject.CreateInstance<Item>();
            generatedItem.ItemName = name;
            generatedItem.ItemSprite = itemSprite;
            generatedItem.DroppedItemSprite = droppedItemSprite;
            generatedItem.Type = type;
            generatedItem.Points = points;
            generatedItem.DroppedItemBase = droppedItemReference;
            return generatedItem;
        }
        
        
        
    }
}
