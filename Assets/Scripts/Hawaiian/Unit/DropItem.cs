using UnityEngine;

namespace Hawaiian.Unit
{
    public class DropItem : MonoBehaviour
    {
        private IUnit user;

        public void Initialise(IUnit user) 
        {
            this.user = user;
        }
        
        public void OnTriggerEnter2D(Collider2D col)
        {
            // TODO: Duplicate code. See DamageIndicator.OnTriggerEnter2D
            if (col.gameObject.GetComponent<Unit>() is IUnit)
            {
                //Yucky 
                IUnit target = (IUnit) col.gameObject.GetComponent<Unit>();

                if (target == user)
                    return;
                
                // TODO: Call drop item function here
            }
        }
    }
}