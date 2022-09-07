using Hawaiian.PositionalEvents;
using Hawaiian.Unit;
using UnityEngine;

namespace Hawaiian.Inventory.HeldItemBehaviours
{
    public class DetonatorHeldItem : HeldItemBehaviour
    {
        [SerializeField] private GameObject _detonatorPrefab;
        public PositionalEventToken DetonateBase;

        private bool CanPlaceDetonator = true;

        public void BeginDetonation()
        {
            if (!CanPlaceDetonator)
                return;

            GameObject detonator = Instantiate(_detonatorPrefab, transform.position, Quaternion.identity);
            detonator.GetComponent<Detonator>().PlayerReference = this.GetComponentInParent<IUnit>();
            DestroyHeldItem();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var listeners = col.GetComponents<PositionalEventListener>();

            foreach (var listener in listeners)
            {
                if (listener == null) continue;

                if (listener.token != DetonateBase) continue;

                if (listener.gameObject.GetComponent<PlayerTreasure>().playerInventoryController !=
                    gameObject.GetComponentInParent<IUnit>())
                {
                    if (listener.gameObject.GetComponent<PlayerTreasure>().DetonatorReference != null)
                        CanPlaceDetonator = false;
                    else
                        CanPlaceDetonator = true;

                }  else
                    CanPlaceDetonator = false;
           
             
            }
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            var listeners = col.GetComponents<PositionalEventListener>();

            foreach (var listener in listeners)
            {
                if (listener == null) continue;

                if (listener.token != DetonateBase) continue;

                if (listener.gameObject.GetComponent<PlayerTreasure>().playerInventoryController !=
                    gameObject.GetComponentInParent<IUnit>())
                {
                    if (listener.gameObject.GetComponent<PlayerTreasure>().DetonatorReference != null)
                        CanPlaceDetonator = false;
                    else
                        CanPlaceDetonator = true;

                }  else
                    CanPlaceDetonator = false;
            }
        }


        private void OnTriggerExit2D(Collider2D col)
        {
            var listeners = col.GetComponents<PositionalEventListener>();

            foreach (var listener in listeners)
            {
                if (listener == null) continue;

                if (listener.token != DetonateBase) continue;

                if (listener.gameObject.GetComponent<PlayerTreasure>().playerInventoryController !=
                    gameObject.GetComponentInParent<IUnit>())
                {
                    if (listener.gameObject.GetComponent<PlayerTreasure>().DetonatorReference != null)
                        CanPlaceDetonator = false;
                    else
                        CanPlaceDetonator = true;

                }
                else
                    CanPlaceDetonator = false;
            }
        }
    }
}