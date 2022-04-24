using UnityEngine;

namespace Hawaiian.Inventory
{
    public class tempItem : MonoBehaviour
    {
        public Item item;

        public void OnPickUp()
        {
            Destroy(gameObject);
        }
    }
}
