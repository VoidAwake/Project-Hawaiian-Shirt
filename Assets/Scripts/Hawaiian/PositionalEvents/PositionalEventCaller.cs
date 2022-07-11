using System;
using System.Collections.Generic;
using System.Linq;
using Hawaiian.Utilities;
using MoreLinq;
using UnityEngine;
using UnityEngine.Events;

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

        [SerializeField] private UnityEvent raisedOnTarget = new UnityEvent();

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
            var listeners = col.GetComponents<PositionalEventListener>();

            
            foreach(var listener in listeners)
            {
                if (listener == null) continue;

                if (listener.token != token) continue;
            
                RegisterListener(listener);
            }
      
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var listeners = other.GetComponents<PositionalEventListener>();

            foreach(var listener in listeners)
            {
                if (listener == null) continue;

                if (listener.token != token) continue;
            
                if (!interactablesInRange.Contains(listener)) continue;
            
                UnregisterListener(listener);
            }
       
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
            var currentTargets = Targets.ToList();
            
            foreach (var target in currentTargets)
            {
                target.response.Invoke();
            }
            
            if (currentTargets.Count > 0)
                raisedOnTarget.Invoke();
        }

        public void Raise(PositionalEventListener target)
        {
            if (!Targets.Contains(target)) throw new ArgumentException($"Target {target} is not in {nameof(Targets)}");
            
            target.response.Invoke();
            
            raisedOnTarget.Invoke();
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