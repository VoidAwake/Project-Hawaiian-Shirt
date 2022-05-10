using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.PositionalEvents
{
    public class TargetControlPrompt : MonoBehaviour
    {
        [SerializeField] private GameObject controlPromptPrefab;
        [SerializeField] private PositionalEventToken token;
        [SerializeField] private string actionName;

        private readonly Dictionary<PositionalEventCaller, ControlPrompt> controlPrompts = new();

        public void OnTargetsChanged(PositionalEventCaller caller)
        {
            if (caller.Token != token) return;
            
            if (!controlPrompts.ContainsKey(caller) && caller.Targets.Count > 0)
                AddControlPrompt(caller);
            else if (controlPrompts.ContainsKey(caller) && caller.Targets.Count > 0)
                UpdateControlPrompt(caller);
            else if (controlPrompts.ContainsKey(caller) && caller.Targets.Count == 0)
                RemoveControlPrompt(caller);
        }

        private void AddControlPrompt(PositionalEventCaller caller)
        {
            var highlighterObject = Instantiate(controlPromptPrefab, transform);

            var controlPrompt = highlighterObject.GetComponent<ControlPrompt>();

            // TODO: Deserves another look...
            var playerInput = caller.GetComponentInParent<PlayerInput>();

            if (playerInput == null) throw new Exception($"{nameof(PositionalEventCaller)} does not have a {nameof(PlayerInput)} component.");

            controlPrompt.Initialise(playerInput, actionName);

            controlPrompts.Add(caller, controlPrompt);
            
            UpdateControlPrompt(caller);
        }

        private void UpdateControlPrompt(PositionalEventCaller caller)
        {
            var highlighterObject = controlPrompts[caller].gameObject;

            highlighterObject.transform.position = AverageTargetPosition(caller);
        }

        private void RemoveControlPrompt(PositionalEventCaller caller)
        {
            Destroy(controlPrompts[caller].gameObject);

            controlPrompts.Remove(caller);
        }

        private Vector3 AverageTargetPosition(PositionalEventCaller caller)
        {
            var average = Vector3.zero;
            
            foreach (var target in caller.Targets)
            {
                average += target.transform.position;
            }

            average /= caller.Targets.Count;

            return average;
        }
    }
}
