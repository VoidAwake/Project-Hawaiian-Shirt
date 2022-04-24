using UnityEngine;

namespace Hawaiian.Inventory
{
    public interface IItem
    {
        public void DropItem();

        public void UseItem();

        public void UseItemAlternate();

        public float GetDamage();

    }
}
