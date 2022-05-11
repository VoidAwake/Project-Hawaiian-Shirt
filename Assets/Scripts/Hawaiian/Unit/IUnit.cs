using UnityEngine;

namespace Hawaiian.Unit
{
    public interface IUnit
    {
        public Vector3 GetPosition();
        
        public void TakeDamage(float damage);

        public void OnDeath();

        public void Use();

        public void ApplyKnockback(Vector2 direction, float distance);

    }
}
