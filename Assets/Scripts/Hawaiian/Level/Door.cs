using UnityEngine;

namespace Hawaiian.Level
{
    public class Door : MonoBehaviour
    {
        Animator animator;
        public bool isLocked = true;
        [SerializeField] public bool inRange = false;
        public BoxCollider2D doorRange;
        
        void Start()
        {
            animator = GetComponentInChildren<Animator>();
            doorRange = GetComponent<BoxCollider2D>();
        }

        public void Unlock()
        {
            animator.SetTrigger("Unlock");
            Destroy(doorRange);
        }
    }
}
