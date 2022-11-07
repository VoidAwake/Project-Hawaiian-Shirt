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

                OnItemChanged();
            }
        }

        private void Awake()
        {
            OnItemChanged();
        }

        public void Initialise(Item item)
        {
            Item = item;
        }
        
        private void Start()
        {
            // Must be called in start to validate all items after they are initialised
            // TODO: Might be possible to refactor now that highlighter has been refactored
            ValidateItem();
        }

        public void OnPickUp()
        {
            Destroy(gameObject);
        }

        public void OnItemChanged()
        {
            if (Item == null) return;
            
            name = Item.name + " Item";

            if (spriteRenderer == null) return;

            spriteRenderer.sprite = Item.DroppedItemSprite;

            spriteRenderer.color = Item.IsDetonator ? Color.red : Color.white;
        }

        private void ValidateItem()
        {
            if (Item != null) return;
            
            Debug.LogWarning($"{nameof(DroppedItem)} has not been assigned an {nameof(Item)}. Disabling.");
                
            gameObject.SetActive(false);
        }
    }
}