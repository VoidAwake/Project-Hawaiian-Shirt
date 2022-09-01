using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.PositionalEvents
{
    [RequireComponent(typeof(Collider2D))]
    public class PositionalEventListener : MonoBehaviour
    {
        public PositionalEventToken token;
        public UnityEvent response;

        public UnityEvent onTargetRemoved;
        
        public bool removeHighlighting = false;
    
    }
}