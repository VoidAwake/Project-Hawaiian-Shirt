using Hawaiian.PositionalEvents;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory
{
    public class HeldItem : MonoBehaviour
    {
        [SerializeField] private UnityEvent itemUsedDown;
        [SerializeField] private UnityEvent itemUsed;
        [SerializeField] private GameObject _detonatorPrefab;
        public PositionalEventToken DetonateBase;

        public UnityEvent destroyed = new UnityEvent();

        public Item Item;

        private bool CanPlaceDetonator = true;

        public bool IsDetonator = false;
        public void Use()
        {
            itemUsed.Invoke();
        }

        public void UseDown()
        {
            itemUsedDown.Invoke();
        }

        public void Destroy()
        {
            destroyed.Invoke();
        }

        public void DestroyDetonator()
        {
            destroyed.Invoke();
        }

        public void BeginDetonation()
        {
            if (!CanPlaceDetonator)
                return;

            GameObject detonator = Instantiate(_detonatorPrefab, transform.position, Quaternion.identity);
            detonator.GetComponent<Detonator>().PlayerReference = this.GetComponentInParent<IUnit>();
            DestroyDetonator();
            
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!IsDetonator)
                return;
            
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
            if (!IsDetonator)
                return;
            
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
            if (!IsDetonator)
                return;
            
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

        public UnityEvent initialised = new();

        public void Initialise(Item currentItem)
        {
            Item = currentItem;
            
            initialised.Invoke();
        }
    }
}