using UnityEngine;

namespace Hawaiian.Inventory
{
    public class BombExplosion : MonoBehaviour
    {
        private float timer = 0;

        public void Update()
        {
            if (timer < 1)
                timer += Time.deltaTime;
            else
                DestoryExplosion();
        }

        public void DestoryExplosion()
        {
            Destroy(gameObject);
        }
    }
}