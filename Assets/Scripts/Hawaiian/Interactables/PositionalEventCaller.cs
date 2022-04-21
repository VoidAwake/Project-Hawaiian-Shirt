using System.Collections.Generic;
using System.Linq;
using Hawaiian.Utilities;
using MoreLinq;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Interactables
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class PositionalEventCaller : MonoBehaviour
    {
        public enum InteractionTarget { All, Closest }

        [SerializeField] private PositionalEventToken token;
        [SerializeField] private GameEvent positionalEventCallerEnabled;
        [SerializeField] private GameEvent positionalEventCallerDisabled;
        [SerializeField] private float radius;
        [SerializeField] private InteractionTarget interactionTarget;

        public UnityEvent<PositionalEventListener> targetAdded = new();
        public UnityEvent<PositionalEventListener> targetRemoved = new();

        private List<PositionalEventListener> interactablesInRange = new();

        private List<PositionalEventListener> Targets { get; set; } = new();

        private void Awake()
        {
            GetComponent<CircleCollider2D>().radius = radius;
        }

        private void Update()
        {
            if (interactionTarget != InteractionTarget.Closest) return;

            var nearest = NearestInteractable();
                
            if (nearest == null)
                Targets = new List<PositionalEventListener>();
            else
                Targets = new List<PositionalEventListener> { NearestInteractable() };
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var listener = col.GetComponent<PositionalEventListener>();

            if (listener == null) return;

            if (listener.token != token) return;
            
            RegisterListener(listener);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var listener = other.GetComponent<PositionalEventListener>();

            if (listener == null) return;

            if (listener.token != token) return;
            
            if (!interactablesInRange.Contains(listener)) return;
            
            UnregisterListener(listener);
        }

        private PositionalEventListener NearestInteractable()
        {
            if (interactablesInRange.Count == 0) return null;
            
            return interactablesInRange.MinBy(i => Vector3.Distance(i.transform.position, transform.position));
        }

        public void RegisterListener(PositionalEventListener listener)
        {
            interactablesInRange.Add(listener);

            if (interactionTarget == InteractionTarget.All)
                Targets = interactablesInRange;
        }

        public void UnregisterListener(PositionalEventListener listener)
        {
            interactablesInRange.Remove(listener);

            if (interactionTarget == InteractionTarget.All)
                Targets = interactablesInRange;
        }

        [ContextMenu("Raise")]
        public void Raise()
        {
            foreach (var target in Targets.ToList())
            {
                target.response.Invoke();
            }
        }
    }
}