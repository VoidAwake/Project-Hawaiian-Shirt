using UnityEngine;

namespace Hawaiian.Inventory
{
    public class DroppedItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private Item item;

        public Item Item
        {
            get => item;
            set
            {
                item = value;

                if (spriteRenderer == null)
                    return;

                // TODO: Duplicate code. See OnValidate.
                spriteRenderer.sprite = item.DroppedItemSprite;
                
                spriteRenderer.color = item.IsDetonator ? Color.red : Color.white;
            }
        }

      

        public void OnPickUp()
        {
            Destroy(gameObject);
        }

        private void OnValidate()
        {
            if (Item == null) return;

            name = Item.name + " Item";

            if (spriteRenderer == null) return;

            // TODO: Duplicate code. See Item.
            spriteRenderer.sprite = Item.DroppedItemSprite;
            
            spriteRenderer.color = Item.IsDetonator ? Color.red : Color.white;
        }
    }
}