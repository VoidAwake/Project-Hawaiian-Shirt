using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class ItemTrap : HeldItemBehaviour
    {
        protected override void UseItemActionPerformed(InputAction.CallbackContext value)
        {
            Cursor.CurrentRad = Item.PlacementRadius;

            GameObject instanceCircle = new GameObject();
            SpriteRenderer renderer = instanceCircle.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            renderer.sprite = Resources.Load<Sprite>("Circle");
            var o = renderer.gameObject;
            o.transform.parent = UnitPlayer.transform;
            o.transform.localPosition = Vector3.zero;
            renderer.gameObject.transform.localScale = new Vector3(Item.PlacementRadius, Item.PlacementRadius, 0);
            renderer.color = new Color32(255, 109, 114, 170);
            renderer.sortingOrder = 1;
        }
    }
}