using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.PositionalEvents
{
    public class TargetControlPrompt : MonoBehaviour
    {
        [SerializeField] private PositionalEventCaller caller;
        [SerializeField] private GameObject controlPromptPrefab;
        [SerializeField] private string actionName;

        private GameObject controlPromptObject;
        private ControlPrompt controlPrompt;

        private void Awake()
        {
            controlPromptObject = Instantiate(controlPromptPrefab, transform);

            controlPrompt = controlPromptObject.GetComponent<ControlPrompt>();

            // TODO: Deserves another look...
            var playerInput = caller.GetComponentInParent<PlayerInput>();

            if (playerInput == null) throw new Exception($"{nameof(PositionalEventCaller)} does not have a {nameof(PlayerInput)} component.");

            controlPrompt.Initialise(playerInput, actionName);

            controlPromptObject.SetActive(false);
        }

        private void OnEnable()
        {
            caller.targetsChanged.AddListener(OnTargetsChanged);
        }

        private void OnDisable()
        {
            caller.targetsChanged.RemoveListener(OnTargetsChanged);
        }

        private void OnTargetsChanged()
        {
            if (caller.Targets.Count > 0)
                UpdateControlPrompt();
            else
                controlPromptObject.SetActive(false);
        }

        private void UpdateControlPrompt()
        {
            controlPromptObject.transform.position = AverageTargetPosition();
            
            controlPromptObject.SetActive(true);
        }

        private Vector3 AverageTargetPosition()
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
