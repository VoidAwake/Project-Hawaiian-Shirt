using System;
using System.Linq;
using Hawaiian.UI.CharacterSelect;
using UI.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace Hawaiian.UI.General
{
    public class OnAnyButtonPress : DialogueComponent<Dialogue>
    {
        public UnityEvent anyButtonPressed = new UnityEvent();

        private IDisposable eventListener;

        private bool wasDialoguePromotedLastFrame;

        private void Update()
        {
            wasDialoguePromotedLastFrame = dialogue.GetComponent<CanvasGroup>().interactable;
        }
        
        protected override void Subscribe()
        {
            InputSystem.onEvent += OnAnyButtonPressed;
        }

        protected override void Unsubscribe()
        {
            InputSystem.onEvent -= OnAnyButtonPressed;
        }
        
        // TODO: Duplicate code. See LobbyPlayerController.
        private void OnAnyButtonPressed(InputEventPtr eventPtr, InputDevice device)
        {
            if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>()) return;

            // TODO: This would be a really good solution, but the players haven't joined yet.
            // if (!PlayerInput.all.SelectMany(a => a.devices).Contains(device)) return;
            
            // Copied from InputUser, works somehow.
            foreach (var control in eventPtr.EnumerateChangedControls(device: device, magnitudeThreshold: 0.0001f))
            {
                if (control == null || control.synthetic || control.noisy) continue;

                if (control is not ButtonControl) return;

                // TODO: We're on;y having to do this because there's no way to access the promoted state of a dialogue
                // TODO: Shouldn't all DialogeCompoentns unsubscribe when the dialogue gets demoted?
                if (wasDialoguePromotedLastFrame)
                    anyButtonPressed.Invoke();
                
                break;
            }
        }
    }
}