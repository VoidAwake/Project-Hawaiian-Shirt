using UnityEngine;

namespace Hawaiian.Inventory
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ShieldCollider : MonoBehaviour
    {
        public Projectile ProjectileInstance;
        public Throwable ThrowableInstance;


        [SerializeField] private IUnitGameEvent _parryOccured;

        private void OnTriggerEnter2D(Collider2D col)
        {
            //TODO: Update itembehaviour to have a reference to their user
            if (col.gameObject.GetComponent<Projectile>() != null)
            {
                ProjectileInstance = col.gameObject.GetComponent<Projectile>();
                _parryOccured.Raise(col.gameObject.GetComponent<Projectile>().User);
            }
            else if (col.gameObject.GetComponent<DamageIndicator>() != null)
                _parryOccured.Raise(col.gameObject.GetComponent<DamageIndicator>().User);
            else if (col.gameObject.GetComponent<Throwable>() != null)
            {
                ThrowableInstance = col.gameObject.GetComponent<Throwable>();
                _parryOccured.Raise(col.gameObject.GetComponent<Throwable>().User);
            }
            
            Destroy(this);
            
        }
    }
}