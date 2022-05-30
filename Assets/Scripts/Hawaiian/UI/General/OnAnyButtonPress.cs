using System;
using UI.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
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
            eventListener = InputSystem.onAnyButtonPress.Call(ctrl =>
            {
                // TODO: We're on;y having to do this because there's no way to access the promoted state of a dialogue
                // TODO: Shouldn't all DialogeCompoentns unsubscribe when the dialogue gets demoted?
                if (wasDialoguePromotedLastFrame)
                    anyButtonPressed.Invoke();
            });
        }

        protected override void Unsubscribe()
        {
            eventListener.Dispose();
        }
    }
}