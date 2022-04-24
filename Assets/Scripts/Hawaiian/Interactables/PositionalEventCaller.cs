using System.Collections.Generic;
using System.Linq;
using Hawaiian.Utilities;
using MoreLinq;
using UnityEngine;

namespace Hawaiian.Interactables
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class PositionalEventCaller : MonoBehaviour
    {
        public enum InteractionTarget { All, Closest }

        [SerializeField] private PositionalEventToken token;
        [SerializeField] private float radius;
        [SerializeField] private InteractionTarget interactionTarget;
        [SerializeField] private BaseGameEvent<PositionalEventCaller> targetsChanged; 

        private List<PositionalEventListener> interactablesInRange = new();
        private List<PositionalEventListener> targets = new();

        public List<PositionalEventListener> Targets
        {
            get => targets;
            set
            {
                targets = value;
                targetsChanged.Raise(this);
            }
        }

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
                Targets = new List<PositionalEventListener> { nearest };
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

        private void RegisterListener(PositionalEventListener listener)
        {
            interactablesInRange.Add(listener);

            if (interactionTarget == InteractionTarget.All)
                Targets = interactablesInRange;
        }

        private void UnregisterListener(PositionalEventListener listener)
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