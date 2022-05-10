using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Hawaiian.PositionalEvents
{
    public class ControlPrompt : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private string actionName;
        [Tooltip("Used if the PlayerInput is not initialised.")]
        [SerializeField] private string defaultControlScheme;
        [SerializeField] private ControlSpriteMappings controlSpriteMappings;
        [SerializeField] private bool intialiseOnAwake;
        [SerializeField] private GameObject controlPromptDisplayPrefab;

        private List<GameObject> controlPromptDisplayObjects = new List<GameObject>();

        private void Awake()
        {
            if (intialiseOnAwake)
                Initialise();
        }

        public void Initialise(PlayerInput playerInput, string actionName)
        {
            this.playerInput = playerInput;
            this.actionName = actionName;

            Initialise();
        }
        
        private void Initialise()
        {
            var action = inputActions.FindAction(actionName, false);

            if (action == null)
            {
                Debug.LogWarning($"No action {actionName} in {inputActions.name} {nameof(InputActionAsset)}.");
                return;
            }

            var controlScheme = playerInput != null ? playerInput.currentControlScheme : defaultControlScheme;

            var inputBinding = action.bindings.FirstOrDefault(a => a.groups.Contains(controlScheme));

            if (inputBinding == default)
            {
                Debug.LogWarning($"No binding for control scheme {controlScheme} in action {action.name}.");
                return;
            }

            if (inputBinding.isPartOfComposite)
            {
                // Find the composite binding
                var bindingIndex = action.bindings.IndexOf(i => i == inputBinding);
                
                while (bindingIndex >= 0)
                {
                    if (action.bindings[bindingIndex].isComposite)
                        break;
                    
                    bindingIndex--;
                }

                bindingIndex++;
                
                // Display all of the composite parts
                while (bindingIndex <= action.bindings.Count - 1)
                {
                    if (!action.bindings[bindingIndex].isPartOfComposite)
                        break;

                    DisplayBinding(action.bindings[bindingIndex]);

                    bindingIndex++;
                }
            }
            else
            {
                DisplayBinding(inputBinding);
            }
        }

        private void DisplayBinding(InputBinding inputBinding)
        {
            var controlPath = inputBinding.path;

            if (!controlSpriteMappings.controlSpriteMappings.ContainsKey(controlPath))
            {
                Debug.LogWarning($"No sprite mapping exists for the control {controlPath}.");
                return;
            }

            var controlPromptDisplayObject = Instantiate(controlPromptDisplayPrefab, transform);

            var spriteRenderer = controlPromptDisplayObject.GetComponent<SpriteRenderer>();
            var image = controlPromptDisplayObject.GetComponent<Image>();
            
            if (spriteRenderer != null)
                spriteRenderer.sprite = controlSpriteMappings.controlSpriteMappings[controlPath];

            if (image != null)
                image.sprite = controlSpriteMappings.controlSpriteMappings[controlPath];
        }
    }
}