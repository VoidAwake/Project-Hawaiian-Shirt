using UnityEngine;

namespace Hawaiian.Unit
{
    public interface IUnit
    {
        public int PlayerNumber { get; set; }
        
        public Vector3 GetPosition();
        
        public void TakeDamage(float damage);

        public void OnDeath();

        public void Use();

        public void ApplyKnockback(Vector2 direction, float distance, float duration = 0.5f);
        
        public void TripUnit(Vector2 direction, float distance);
        public UnitPlayer GetUnit();
        
        public Color32 PlayerColour { get; set; }


    }
}
