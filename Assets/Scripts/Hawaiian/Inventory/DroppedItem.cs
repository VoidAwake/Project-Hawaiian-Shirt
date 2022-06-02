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

                spriteRenderer.sprite = Item.DroppedItemSprite;
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

            spriteRenderer.sprite = Item.DroppedItemSprite;
        }
    }
}