using UnityEngine;

namespace Hawaiian.Inventory
{
    public class DroppedItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public Item item;

        public void OnPickUp()
        {
            Destroy(gameObject);
        }

        private void OnValidate()
        {
            if (item == null) return;

            name = item.name + " Item";

            if (spriteRenderer == null) return;

            spriteRenderer.sprite = item.itemSprite;
        }
    }
}
