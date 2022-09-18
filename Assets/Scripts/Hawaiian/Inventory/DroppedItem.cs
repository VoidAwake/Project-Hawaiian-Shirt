using UnityEngine;
using Application = UnityEngine.Device.Application;

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

                OnItemChanged();
            }
        }

        private void Start()
        {
            OnItemChanged();
        }

        public void OnPickUp()
        {
            Destroy(gameObject);
        }

        public void OnItemChanged()
        {
            if (Item == null)
            {
                if (!Application.isPlaying) return;
                
                Debug.LogWarning($"{nameof(DroppedItem)} has not been assigned an {nameof(Item)}. Disabling.");
                
                gameObject.SetActive(false);
                
                return;
            }

            name = Item.name + " Item";

            if (spriteRenderer == null) return;

            spriteRenderer.sprite = Item.DroppedItemSprite;

            spriteRenderer.color = Item.IsDetonator ? Color.red : Color.white;
        }
    }
}