using Hawaiian.Unit;
using UnityEngine;

namespace Hawaiian.Inventory
{
    public class ItemTrap : MonoBehaviour
    {
        // TODO: Duplicate code. See ItemInstatiate
        public Item item;
        public UnitPlayer _playerReference;
        protected Cursor cursor;
        private Transform firePoint;
        private HeldItem heldItem;
        
        private void Awake()
        {
            // TODO: Need to consider where we're getting these references
            _playerReference = GetComponentInParent<ItemHolder>().unitPlayer;
            cursor = GetComponentInParent<ItemHolder>().cursor;
            firePoint = GetComponentInParent<ItemHolder>().firePoint;
            heldItem = GetComponent<HeldItem>();
            
            heldItem.initialised.AddListener(OnInitialised);
            
            OnInitialised();
        }

        protected virtual void OnInitialised()
        {
            item = heldItem.Item;
        }

        private void BeginTrapHighlighting()
        {
            cursor.CurrentRad = item.PlacementRadius;

            GameObject instanceCircle = new GameObject();
            SpriteRenderer renderer = instanceCircle.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            renderer.sprite = Resources.Load<Sprite>("Circle");
            var o = renderer.gameObject;
            o.transform.parent = _playerReference.transform;
            o.transform.localPosition = Vector3.zero;
            renderer.gameObject.transform.localScale = new Vector3(item.PlacementRadius, item.PlacementRadius, 0);
            renderer.color = new Color32(255, 109, 114, 170);
            renderer.sortingOrder = 1;
        }
    }
}