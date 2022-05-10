using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hawaiian.PositionalEvents
{
    public class ControlPrompt : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private InputAction action;
        [SerializeField] private string actionName;
        [SerializeField] private ControlSpriteMappings controlSpriteMappings;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private InputActionAsset inputActions;

        public void Initialise(PlayerInput playerInput, string actionName)
        {
            this.playerInput = playerInput;
            this.actionName = actionName;
            
            action = inputActions.FindAction(actionName, false);

            var inputBinding = action.bindings.First(a => a.groups.Contains(playerInput.currentControlScheme));
            var controlPath = inputBinding.path;

            if (!controlSpriteMappings.controlSpriteMappings.ContainsKey(controlPath)) return;

            spriteRenderer.sprite = controlSpriteMappings.controlSpriteMappings[controlPath];
        }
    }
}