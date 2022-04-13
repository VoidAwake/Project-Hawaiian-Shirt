using System;
using System.Collections.Generic;
using MoreLinq;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Interactables
{
    public class Interactor : MonoBehaviour
    {
        public enum InteractionTarget { All, Closest }

        [SerializeField] private InteractionTarget interactionTarget;
        private List<Interactable> interactablesInRange = new();

        public List<Interactable> targets = new();

        public UnityEvent<Interactable> targetAdded = new();
        public UnityEvent<Interactable> targetRemoved = new();
        
        private void UpdateTargets()
        {
            targets = interactionTarget switch
            {
                InteractionTarget.All => interactablesInRange,
                InteractionTarget.Closest => new List<Interactable> { NearestInteractable() },
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private Interactable NearestInteractable()
        {
            return interactablesInRange.MinBy(i => Vector3.Distance(i.transform.position, transform.position));
        }

        private void Trigger()
        {
            foreach (var target in targets)
            {
                target.Trigger();
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var interactable = col.GetComponent<Interactable>();

            if (interactable == null) return;
            
            // TODO: Determine if we can interact with this interactable
            
            interactablesInRange.Add(interactable);
            
            UpdateTargets();

            if (targets.Contains(interactable))
            {
                targetAdded.Invoke(interactable);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var interactable = other.GetComponent<Interactable>();
            
            if (interactable == null) return;
            
            // TODO: Determine if we can interact with this interactable

            if (targets.Contains(interactable))
            {
                targetRemoved.Invoke(interactable);
            }

            interactablesInRange.Remove(interactable);
            
            UpdateTargets();
        }
    }
}