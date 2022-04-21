using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Interactables
{
    [RequireComponent(typeof(Collider2D))]
    public class PositionalEventListener : MonoBehaviour
    {
        public PositionalEventToken token;
        public UnityEvent response;
    }
}