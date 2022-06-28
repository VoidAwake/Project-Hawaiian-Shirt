using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Inventory
{
    public class HeldItem : MonoBehaviour
    {
        [SerializeField] private UnityEvent itemUsed;
        [SerializeField] private GameObject _detonatorPrefab;

        public UnityEvent destroyed = new UnityEvent();
        
        public void Use() {
            itemUsed.Invoke();
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
           GameObject detonator =  Instantiate(_detonatorPrefab, transform.position, Quaternion.identity);
           detonator.GetComponent<Detonator>().PlayerReference = this.GetComponentInParent<IUnit>();
        }
    }
}