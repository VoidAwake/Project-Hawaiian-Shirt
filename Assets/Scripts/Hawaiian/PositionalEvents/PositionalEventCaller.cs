using System;
using System.Collections.Generic;
using System.Linq;
using Hawaiian.Utilities;
using MoreLinq;
using UnityEngine;

namespace Hawaiian.PositionalEvents
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

        public PositionalEventToken Token => token;

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
            UpdateTargets();
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

            UpdateTargets();
        }

        private void UnregisterListener(PositionalEventListener listener)
        {
            interactablesInRange.Remove(listener);

            UpdateTargets();
        }

        [ContextMenu("Raise")]
        public void Raise()
        {
            foreach (var target in Targets.ToList())
            {
                target.response.Invoke();
            }
        }

        public void Raise(PositionalEventListener target)
        {
            if (!Targets.Contains(target)) throw new ArgumentException($"Target {target} is not in {nameof(Targets)}");
            
            target.response.Invoke();
        }

        private void UpdateTargets()
        {
            switch (interactionTarget)
            {
                case InteractionTarget.All:
                    Targets = interactablesInRange;
                    break;
                case InteractionTarget.Closest:
                    var nearest = NearestInteractable();
                        
                    if (nearest == null)
                        Targets = new List<PositionalEventListener>();
                    else
                        Targets = new List<PositionalEventListener> { nearest };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}