using UnityEngine;

namespace Hawaiian.Unit
{
    public interface IUnit
    {
        public Vector3 GetPosition();
        
        public void TakeDamage(float damage);

        public void OnDeath();

        public void Use();
    
    }
}
