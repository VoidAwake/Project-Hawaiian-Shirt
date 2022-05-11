using UnityEngine;

namespace Hawaiian.Inventory
{
    public abstract class HitEffect : MonoBehaviour
    {
        public abstract void OnHit(Unit.Unit unit, Vector2 direction);
    }
}