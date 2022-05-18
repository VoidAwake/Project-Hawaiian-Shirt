using UnityEngine;

namespace Hawaiian.Level
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private static readonly int UnlockTrigger = Animator.StringToHash("Unlock");

        public void Unlock()
        {
            animator.SetTrigger(UnlockTrigger);
        }
    }
}
